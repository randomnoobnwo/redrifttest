using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Gameboard.Scripts;
using Gameboard.Scripts.Inputs;
using UnityEngine;
using UnityEngine.UI;

public class GameboardView : MonoBehaviour, IGameBoardView
{
    public Button PlayCardButton;
    public Button RandomChangeButton;
    public Button RandomChangeInfiniteButton;
    public Button DrawRandomCardsButton;
    public Text FireText;
    public Text StatsText;

    private ImmutableQueue<Action> _actionsQueue = ImmutableQueue<Action>.Empty;

    private void Start()
    {
        StartCoroutine(ProcessActionsQueue());
    }

    private IEnumerator ProcessActionsQueue()
    {
        while (true)
        {
            if (_actionsQueue.IsEmpty)
                yield return new WaitForEndOfFrame();
            else
            {
                _actionsQueue = _actionsQueue.Dequeue(out Action action);
            
                action.Invoke();
                yield return new WaitForEndOfFrame();
            }
        }
    }
    
    public void MoveCards(IEnumerable<ICardView> cards, ViewCardLocation loc)
    {
        _actionsQueue = _actionsQueue.Enqueue(() => Debug.LogWarning($"Moving {cards.Count()} cards to {loc}"));
    }

    public ICardView CreateCard(string id, string title, string description, int attack, int healthPoints, int manaCost,
        string artTextureUrl)
    {
        var cardView = gameObject.AddComponent<CardView>();

        cardView.Init(id, title, description, attack, healthPoints, manaCost, artTextureUrl);

        return cardView;
    }

    public void SetupInput(MainInputView input)
    {
        _actionsQueue = _actionsQueue.Enqueue(() =>
        {
            FireText.text = $"Fire: {input.Fire}";
            StatsText.text = input.Stats;

            PlayCardButton.gameObject.SetActive(input.CardInputs.Length > 0);
            PlayCardButton.onClick.RemoveAllListeners();
            PlayCardButton.onClick.AddListener(() =>
            {
                if (input.CardInputs.Length > 0)
                    input.CardInputs[0].OnDrag?.Invoke();
                else
                    Debug.LogWarning("No cards to play");
            });

            SetupButton(RandomChangeButton, input.RandomChange);
            SetupButton(RandomChangeInfiniteButton, input.RandomChangeInfinite);
            SetupButton(DrawRandomCardsButton, input.DrawCards);
        });
    }

    private void SetupButton(Button button, Action action)
    {
        button.onClick.RemoveAllListeners();
        button.gameObject.SetActive(action != null);
        if (action != null)
            button.onClick.AddListener(action.Invoke);
    }

    public void UpdateStats(string stats)
    {
        _actionsQueue = _actionsQueue.Enqueue(() =>
        {
            StatsText.text = stats;
        });
    }

    public void NoInput()
    {
        _actionsQueue = _actionsQueue.Enqueue(() =>
        {
            PlayCardButton.gameObject.SetActive(false);
            RandomChangeButton.gameObject.SetActive(false);
            RandomChangeInfiniteButton.gameObject.SetActive(false);
            DrawRandomCardsButton.gameObject.SetActive(false);
        });
    }

    public void UpdateCardHealth(ICardView cardView, int oldValue, int newValue)
    {
        var cv = cardView as CardView;
        _actionsQueue = _actionsQueue.Enqueue(() =>
        {
            Debug.LogWarning($"Updating {cv.Id}'s health from {oldValue} to {newValue}");
        });
    }

    public void UpdateCardAttack(ICardView cardView, int oldValue, int newValue)
    {
        var cv = cardView as CardView;
        _actionsQueue = _actionsQueue.Enqueue(() =>
        {
            Debug.LogWarning($"Updating {cv.Id}'s attack from {oldValue} to {newValue}");
        });
    }

    public void UpdateCardManaCost(ICardView cardView, int oldValue, int newValue)
    {
        var cv = cardView as CardView;
        _actionsQueue = _actionsQueue.Enqueue(() =>
        {
            Debug.LogWarning($"Updating {cv.Id}'s mana cost from {oldValue} to {newValue}");
        });
    }

    public void EnqueueAction(Action action)
    {
        _actionsQueue = _actionsQueue.Enqueue(action);
    }
}