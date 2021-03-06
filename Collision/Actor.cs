﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace UmbrellaToolKit.Collision
{
    public class Actor : GameObject
    {
        public String tag;
        public bool active = true;
        public override void UpdateData(GameTime gameTime)
        {
            base.UpdateData(gameTime);
            this.gravity((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
        }

        public int Right {
            get => (int)(this.Position.X + this.size.X);
        }

        public int Left {
            get => (int)(this.Position.X);
        }

        public int Top {
            get => (int)(this.Position.Y);
        }

        public int Bottom {
            get => (int)(this.Position.Y + this.size.Y);
        }

        public enum EDGES { TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT };
        public Dictionary<EDGES, bool> EdgesIsCollision = new Dictionary<EDGES, bool> {
            { EDGES.TOP_LEFT, false },
            { EDGES.TOP_RIGHT, false },
            { EDGES.BOTTOM_LEFT, false },
            { EDGES.BOTTOM_RIGHT, false },
        };

        public void SetFalseAllEdgeCollision()
        {
            this.EdgesIsCollision[EDGES.TOP_LEFT] = false;
            this.EdgesIsCollision[EDGES.TOP_RIGHT] = false;
            this.EdgesIsCollision[EDGES.BOTTOM_LEFT] = false;
            this.EdgesIsCollision[EDGES.BOTTOM_RIGHT] = false;
        }

        public bool AnyCollisionRamps()
        {
            return this.EdgesIsCollision[EDGES.TOP_LEFT] ||
                this.EdgesIsCollision[EDGES.TOP_RIGHT] ||
                this.EdgesIsCollision[EDGES.BOTTOM_LEFT] ||
                this.EdgesIsCollision[EDGES.BOTTOM_RIGHT];
        }

        public Vector2 gravity2D = new Vector2(0, -350);
        public Vector2 velocity = new Vector2(0,0);
        public float velocityDecrecentY = 200;
        public float velocityDecrecentX = 200;
        private void gravity(float DeltaTime)
        {

            Vector2 gravity = new Vector2(this.velocity.X + this.gravity2D.X, this.velocity.Y + this.gravity2D.Y);

            if (gravity.X != 0)
                this.moveX((int)(-(gravity.X * DeltaTime)), this.OnCollision);
            if (gravity.Y != 0)
                this.moveY((int)(-(gravity.Y * DeltaTime)), this.OnCollision);

            // velocity Controller
            if (this.velocity.Y > 0)
                this.velocity.Y -= this.velocityDecrecentY * DeltaTime;
            else if (this.velocity.Y < 0)
                this.velocity.Y += this.velocityDecrecentY * DeltaTime;
            else
                this.velocity.Y = 0;

            if (this.velocity.X > 0)
                this.velocity.X -= this.velocityDecrecentX * DeltaTime;
            else if (this.velocity.X < 0)
                this.velocity.X += this.velocityDecrecentX * DeltaTime;
            else
                this.velocity.X = 0;
            // end velocity Controller
        }

        float xRemainder = 0;
	    public void moveX(float amount, Action<string?> onCollideFunction = null)
        {
		    xRemainder += amount;
  		    int move = (int)Math.Round(xRemainder);

		    if (move != 0) 
		    {	
			    xRemainder -= move; 
			    int sign = Math.Sign(move); 
			    while (move != 0) 
			    {
                    Vector2 _position = new Vector2(this.Position.X+ sign, this.Position.Y);
				    if (!collideAt(this.Scene.AllSolids, _position) || AnyCollisionRamps())
				    {
                        if (this.EdgesIsCollision[EDGES.BOTTOM_RIGHT] && (sign > 0 || gravity2D.Y == 0 ))
                            this.Position.Y -= sign;

                        if (this.EdgesIsCollision[EDGES.BOTTOM_LEFT] && (sign < 0 || gravity2D.Y == 0))
                            this.Position.Y += sign;

                        this.Position.X += sign; 
					    move -= sign;
				    } 
				    else 
				    {
					    if(onCollideFunction != null)
						    onCollideFunction(null);
					    break; 
				    } 
			    } 
		    } 
	    }

        float yRemainder = 0;
        public void moveY(float amount, Action <string?> onCollideFunction = null){
	        yRemainder += amount;
  	        int move = (int)Math.Round(yRemainder);

	        if (move != 0) 
	        { 
		        yRemainder -= move; 
		        int sign = Math.Sign(move); 
		        while (move != 0) 
		        {
                    Vector2 _position = new Vector2(this.Position.X, this.Position.Y+ sign);
			        if (!collideAt(this.Scene.AllSolids, _position))
			        { 

                        this.Position.Y += sign; 
                        move -= sign; 

                    } 
			        else 
			        { 
				        if(onCollideFunction != null)
					        onCollideFunction(null);
                        break; 
			        } 
		        } 
	        } 
        }

	    public bool overlapCheck(Actor actor){
            bool AisToTheRightOfB = actor.Left >= this.Right;
            bool AisToTheLeftOfB = actor.Right <= this.Left;
            bool AisAboveB = actor.Bottom <= this.Top;
            bool AisBelowB = actor.Top >= this.Bottom;
            return !(AisToTheRightOfB
                || AisToTheLeftOfB
                || AisAboveB
                || AisBelowB);
        }

        public bool overlapCheckPixel(Actor actor)
        {
            bool AisToTheRightOfB = actor.Left > this.Right;
            bool AisToTheLeftOfB = actor.Right < this.Left;
            bool AisAboveB = actor.Bottom < this.Top;
            bool AisBelowB = actor.Top > this.Bottom;
            return !(AisToTheRightOfB
                || AisToTheLeftOfB
                || AisAboveB
                || AisBelowB);
        }

        private bool collideAt(List<Solid>solids, Vector2 position){
		    bool rt = false;
		    foreach(Solid solid in solids){
			    if(solid.check(this.size, position)){
                    rt = true;
                }
            }
            if (this.Scene.Grid.checkOverlap(this.size, position, this))
                rt = true;

		    return rt;
	    }

	    public virtual bool isRiding(Solid solid){
		    if(solid.check(this.size, new Vector2(this.Position.X, this.Position.Y + 1)))
			    return true;

		    return false;
	    }

        public virtual bool isRidingGrid(Grid grid)
        {
            if (grid.checkOverlap(this.size, new Vector2(this.Position.X, this.Position.Y + 1), this))
                return true;

            return false;
        }

        public virtual void squish(string tag = null){}
    }
}
