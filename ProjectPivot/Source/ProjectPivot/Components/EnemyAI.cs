using System;
using Microsoft.Xna.Framework;
using ProjectPivot.Entities;
using ProjectPivot.Components.AI;
using ProjectPivot.Pathfinding;
using FarseerPhysics;
using ProjectPivot.Utils;

namespace ProjectPivot.Components {
    public class EnemyAI : Component {
        EnemyInput input;
        AiMotion motion;
        AiVision vision;
        AStar path;
        Map map;

        Cell targetCell;
        Cell nextCell;
        public EnemyAI() {
        }

        public override void Initialize() {
            map = Map.Current;
            input = GameObject.GetComponent<EnemyInput>();
        }

        public override void Update(GameTime gameTime) {
            Cell currentCell = map.CellAtWorld(GameObject.Position);
            if (targetCell == null) {
                targetCell = map.RandomHollowCell();
                Console.WriteLine("Retargeting");
                path = new AStar(map, currentCell, targetCell);
            }
            if (nextCell == null) {
                nextCell = path.Dequeue();
                if (nextCell == null) {
                    targetCell = null;
                }
                if (nextCell == targetCell) {
                    targetCell = null;
                    nextCell = null;
                }
            }
            if (nextCell == null) {
                input.Heading = Vector2.Zero;
            } else {
                if (nextCell == currentCell || nextCell.IsNeighbour(currentCell)) {
                    // on to next cell
                    nextCell = null;
                } else {
                    Gizmo.Rectangle(targetCell.Area, Color.Red);
                    Gizmo.Rectangle(nextCell.Area, Color.Green);
                    // position between where we are and where we want to be
                    input.Heading = nextCell.Position - GameObject.Position;
                    input.Heading.Normalize();
                    //input.Heading = ConvertUnits.ToSimUnits(input.Heading);
                }
            }
        }
    }
}
