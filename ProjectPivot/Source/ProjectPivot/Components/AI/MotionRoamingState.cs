using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ProjectPivot.Pathfinding;

namespace ProjectPivot.Components.AI {
    public class MotionRoamingState : EnemyState {
        private static Random random = new Random();
        private Cell destination;
        private Cell currentCell;
        private Cell nextCell;
        private AStar path;
        private EnemyInput input;
        public MotionRoamingState(Enemy enemy) : base(enemy) {
            input = enemy.GetComponent<EnemyInput>();
            input.InMotion = true;
            setCurrentCell();
            List<Cell> healthyCells = enemy.Vision.HealthyCells();
            destination = healthyCells[random.Next(healthyCells.Count)] as Cell;
            AStar path = new AStar(Map.Current, currentCell, destination);
            if (path.Length > 0) {
                this.path = path;
            }
        }
        public override EnemyState Update(GameTime gameTime) {
            if (path == null) {
                return nextState();
            }
            if (nextCell == null) {
                nextCell = path.Dequeue();
            }
            if (nextCell == null) {
                return nextState();
            }
            setCurrentCell();
            setCurrentHeading();
            setCurrentDirection();
            return this;
        }

        private void setCurrentCell() {
            currentCell = Map.Current.CellAtWorld(enemy.Position);
        }

        private EnemyState nextState() {
            input.Heading = Vector2.Zero;
            input.InMotion = false;
            return new MotionIdleState(enemy);
        }

        private void setCurrentDirection() {
            input.Rotation = (float)Math.Atan2(input.Heading.Y, input.Heading.X);
        }

        private void setCurrentHeading() {
            if (nextCell == currentCell || (nextCell.IsNeighbour(currentCell, true))) {
                // on to next cell
                nextCell = null;
            } else {
                //Gizmo.Rectangle(targetCell.Area, Color.Red);
                //Gizmo.Rectangle(nextCell.Area, Color.Green);
                // position between where we are and where we want to be
                input.Heading = nextCell.Position - enemy.Position;
                input.Heading.Normalize();
            }
        }
    }
}
