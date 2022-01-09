using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using GameCore;
using GameCore.Logic;
using UnityEngine;
using Random = System.Random;

namespace Gameboard.Scripts
{
    public class BoardStarter : MonoBehaviour
    {
        public GameboardView View;
        private void Start()
        {
            var state = new GameState();
            state = state.With(randomSeed: 11111111, fire: 5);

            var random = new Random((int)DateTime.Now.Ticks);
            
            for (var i = 0; i < 30; i++)
            {
                var cardBase = new CardBase(Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    random.Next(1, 7),
                    random.Next(1, 7),
                    random.Next(1, 7),
                    "https://picsum.photos/512/512");
                state = state.AddCardBase(cardBase);
            }

            var controller = new GameBoardController(state, View);
            
            controller.StartGame();
        }
    }
}