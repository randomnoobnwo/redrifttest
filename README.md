# redrifttest
Red Rift Test Project

# How to test
Run Gameboard scene

- stats on top show deck, hand, play piles counts.
- fire (mana) - shows amount of fire available
- playable cards - cards that cost less or equal to available fire

## buttons
- randomize one or all cards until to cards left in hand.
- play card button plays first playable card from hand.
- draw button draws 5 cards from deck.

# Game Core
Separated into Plugins/Core folder. Should be a submodule to allow testing it without unity, but I didn't have enough time to set that up.

## GameState

Game state object. Contains all information about... game state.

Logic related to operating state located in StateLogic.cs

## GameContext

Contains game state object and list of view actions generated.
Used to performs operations on state.

Logic located in GameLogic.cs as extension methods of GameContext class.

# MVC

## GameboardView

Knows nothing about backend - just a set of game objects, manipulated over IGameboardView interface.

## GameboardController

Main controller which holds game state and references view over interface.
Contains logic related to core/view interaction.

# Main game cycle

- controller instantiated using intial state and view.
- ui reset is performed from initial state to setup board view
- controller generates input object from the state and sends it to the view
- view triggers provided callbacks and controller executes game moves set in MainInputView callbacks
- moves execution generates set of view actions (e.g. move card(s), update card attributes etc) 
- view actions get processed and certain methods of view get called to achieve needed visual result
- after all view actions executed, controller sets up next input for the view
