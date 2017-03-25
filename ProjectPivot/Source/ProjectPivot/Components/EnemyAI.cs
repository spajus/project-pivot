using System;
using Microsoft.Xna.Framework;
using ProjectPivot.Entities;
using ProjectPivot.Components.AI;
using ProjectPivot.Pathfinding;
using FarseerPhysics;
using ProjectPivot.Utils;
using System.Collections.Generic;

namespace ProjectPivot.Components {
    public class EnemyAI : Component {
        EnemyInput input;
        AiMotion motion;
        AiVision vision;
        AStar path;
        Map map;
        Enemy enemy;
        float spentInCurrentCell = 0f;
        float rethinkCooldown = 0f;
        Cell currentCell;
        List<GameObject> nearbyStuff;

        Cell targetCell;
        Cell nextCell;
        private static Random random = new Random(DateTime.Now.Millisecond);
        public EnemyAI() {
        }

        public override void Initialize() {
            enemy = (Enemy)GameObject;
            map = Map.Current;
            input = GameObject.GetComponent<EnemyInput>();
        }

        public override void Update(GameTime gameTime) {
            /*
            if (path != null) {
                foreach (Cell c in path.List()) {
                    Gizmo.Rectangle(c.Area, Color.Blue);
                }
            }
            */


             checkIfStuck(gameTime);

            if (rethinkCooldown <= 0f) {
                updateNearbyStuff();
                rethinkCooldown = 500f;
            }

            checkTargetCell();
            checkNextCell();
            setCurrentHeading();
            setCurrentDirection();

            if (rethinkCooldown > 0f) {
                rethinkCooldown -= gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        private void updateNearbyStuff() {
            nearbyStuff = GameObjects.Nearby<GameObject>(GameObject.Position, 10f * 32);
            if (enemy.Target == null) {
                if (nearbyStuff.Contains(Player.Current)) {
                    // immediately drop target, will retarget to player
                    targetCell = null;
                    nextCell = null;
                }
            }
        }

        private void setCurrentDirection() {
            if (input.Heading != Vector2.Zero) {
                input.Rotation = (float)Math.Atan2(input.Heading.Y, input.Heading.X);
            }
        }

        private void setCurrentHeading() {
            if (nextCell == null) {
                input.Heading = Vector2.Zero;
            } else {
                if (nextCell == currentCell || (nextCell.IsNeighbour(currentCell, true))) {
                    // on to next cell
                    nextCell = null;
                } else {
                    //Gizmo.Rectangle(targetCell.Area, Color.Red);
                    //Gizmo.Rectangle(nextCell.Area, Color.Green);
                    // position between where we are and where we want to be
                    input.Heading = nextCell.Position - GameObject.Position;
                    input.Heading.Normalize();
                    //input.Heading = ConvertUnits.ToSimUnits(input.Heading);
                }
            }
        }

        private void checkNextCell() {
            if (nextCell == null) {
                if (path != null) {
                    nextCell = path.Dequeue();
                }
                if (nextCell == null) {
                    targetCell = null;
                }
                if (nextCell == targetCell) {
                    targetCell = null;
                    nextCell = null;
                }
            }
        }

        private Cell pickTargetCell() {
            if (enemy.Target != null) {
                return map.CellAtWorld(enemy.Target.Position);
            }
            if (enemy.Target == null && nearbyStuff.Contains(Player.Current)) {
                // check if path is possible to current player
                Cell playerCell = map.CellAtWorld(Player.Current.Position);
                AStar ppath = new AStar(Map.Current, currentCell, playerCell);
                if (ppath.Length > 0) {
                    path = ppath;
                    enemy.Target = Player.Current;
                    return playerCell;
                } 
            }
            List<GameObject> healthyCells = nearbyStuff.FindAll(c => (c is Cell) && !((Cell)c).IsHealthy) ;
            return healthyCells[random.Next(healthyCells.Count)] as Cell;
        }

        private void checkTargetCell() {
            if (targetCell == null) {
                targetCell = pickTargetCell();
                if (currentCell.IsNeighbour(targetCell) || currentCell == targetCell) {
                        path = null;
                        // return;
                }
                if (path == null) {
                    path = new AStar(map, currentCell, targetCell);
                }
                if (path.Length == 0) {
                    // oh well
                    path = null;
                    targetCell = null;
                }
            }
        }

        private void checkIfStuck(GameTime gameTime) {
            Cell newCurrentCell = map.CellAtWorld(GameObject.Position);
            if (currentCell == newCurrentCell) {
                spentInCurrentCell += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            } else {
                spentInCurrentCell = 0f;
                currentCell = newCurrentCell;
            }

            if (targetCell != null && spentInCurrentCell > 1000f) {
                Console.WriteLine("Stuck, dropping target");
                nextCell = null;
                targetCell = null;
                spentInCurrentCell = 0f;
            }
        }
    }
}
