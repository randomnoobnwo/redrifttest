using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Gameboard.Scripts;
using GameCore;
using GameCore.GameMoves;
using GameCore.Logic;

public class GameBoardController
{
    private GameState _state;
    private IGameBoardView _view;
    private Dictionary<int, ICardView> _cardViews = new Dictionary<int, ICardView>();

    public GameBoardController(GameState state, IGameBoardView view)
    {
        _state = state;
        _view = view;
    }

    public void StartGame()
    {
        var ctx = new GameContext(_state).StartGame();
        _state = ctx.State;
        ResetView(_state);

        ProcessActions(ctx.Actions, SetupInput);
    }

    private void ResetView(GameState state)
    {
        foreach (var kvp in state.CardBaseMap)
        {
            CreateCard(kvp.Key, kvp.Value);
        }
    }

    private void CreateCard(int cardId, CardBase cardBase)
    {
        if (_cardViews.ContainsKey(cardId))
            return;

        var cardView = _view.CreateCard(cardBase.Id, cardBase.Title, cardBase.Description, cardBase.Attack,
            cardBase.HealthPoints, cardBase.ManaCost, cardBase.ArtTextureUrl);

        _cardViews.Add(cardId, cardView);
    }

    private void ProcessActions(ImmutableQueue<ViewAction> actions, Action onDone)
    {
        if (actions.IsEmpty)
        {
            onDone?.Invoke();
            return;
        }

        var action = actions.Peek();

        ExecuteAction(action, () => { ProcessActions(actions.Dequeue(), onDone); });
    }

    private void ExecuteAction(ViewAction action, Action onDone)
    {
        switch (action)
        {
            case CardHealthUpdate cardHealthUpdate:
                if (cardHealthUpdate.NewValue == cardHealthUpdate.OldValue) break;
                _view.UpdateCardHealth(_cardViews[cardHealthUpdate.Card], cardHealthUpdate.OldValue, cardHealthUpdate.NewValue);
                break;
            case CardAttackUpdate cardAttackUpdate:
                if (cardAttackUpdate.NewValue == cardAttackUpdate.OldValue) break;
                _view.UpdateCardAttack(_cardViews[cardAttackUpdate.Card], cardAttackUpdate.OldValue, cardAttackUpdate.NewValue);
                break;
            case CardManaCostUpdate cardManaCostUpdate:
                if (cardManaCostUpdate.NewValue == cardManaCostUpdate.OldValue) break;
                _view.UpdateCardManaCost(_cardViews[cardManaCostUpdate.Card], cardManaCostUpdate.OldValue, cardManaCostUpdate.NewValue);
                break;
            case MoveCardsAction moveCardsAction:
                _view.MoveCards(GetCardViews(moveCardsAction.Cards), ToViewLoc(moveCardsAction.Target));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action));
        }

        onDone?.Invoke();
    }

    private ViewCardLocation ToViewLoc(CardLocation loc)
    {
        switch (loc)
        {
            case CardLocation.Deck:
                return ViewCardLocation.Deck;
            case CardLocation.Hand:
                return ViewCardLocation.Hand;
            case CardLocation.Play:
                return ViewCardLocation.Play;
            default:
                throw new ArgumentOutOfRangeException(nameof(loc), loc, null);
        }
    }

    private IEnumerable<ICardView> GetCardViews(IEnumerable<int> cards)
    {
        foreach (var card in cards)
        {
            _cardViews.TryGetValue(card, out var returnCard);

            if (returnCard != null) yield return returnCard;
        }
    }

    private void SetupInput()
    {
        _view.SetupInput(InputController.GetInputView(this, _state.GetInput(), id => _cardViews[id]));
    }
    
    public void ExecuteMove(GameMove move)
    {
        _view.NoInput();

        var ctx = new GameContext(_state);

        switch (move)
        {
            case PlayCardMove playCardMove:
                ctx = ctx.PlayCard(playCardMove.CardId);
                break;
            case DrawCardsMove drawCardsMove:
                ctx = ctx.DrawCards(drawCardsMove.Number);
                break;
            case RandomChangeMove randomChangeMove:
                ctx = ctx.RandomChange();

                if (randomChangeMove.TillTheEnd)
                {
                    _state = ctx.State;

                    ProcessActions(ctx.Actions, () =>
                    {
                        if (_state.GetCardsInLoc(CardLocation.Hand).Count == 0)
                            SetupInput();
                        else
                        {
                            var mainInput = _state.GetInput() as MainInput;
                            var stats =
                                $"Playable: {mainInput.PlayableCards.Count} Deck: {mainInput.DeckCount} Hand: {mainInput.HandCount} Play: {mainInput.PlayCount}";
                            _view.UpdateStats(stats);
                            _view.EnqueueAction(() => ExecuteMove(new RandomChangeMove(true)));
                        }
                    });
                    return;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(move));
        }

        _state = ctx.State;

        ProcessActions(ctx.Actions, SetupInput);
    }
}