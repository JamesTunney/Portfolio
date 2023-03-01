class Ship extends PIXI.Sprite {
	constructor(x = 0, y = 0) {
		super(app.loader.resources["images/playerKnightOne.png"].texture);
		this.anchor.set(.5, .5);
		this.scale.set(0.1);
		this.x = x;
		this.y = y;
	}
}

//creates the enemys in the game
class Circle extends PIXI.Graphics{
	constructor(radius, color = 0xFF0000, x=0, y=0, chase = false, shoot = false){
		super();
		this.beginFill(color);
		this.drawCircle(0,0,radius);
		this.endFill();
		this.x = x;
		this.y = y;
		this.radius = radius;
		
		//determin type of movment
		if(!(chase) && shoot){
			this.fwd = {x:1 ,y:0};
		}
		else{
			this.fwd = getRandomUnitVector();
		}
		this.speed = 50;
		this.isAlive = true;
		
		//determin if they shoot and chase player
		this.chase = chase;
		this.fireShot = shoot;
	}
	
	//moves the enemy
	move(dt=1/60, xChange, yChange){
		if(this.chase) {
			this.x += xChange * this.speed * dt;
			this.y += yChange * this.speed * dt;
		}
		else {
			this.x += this.fwd.x * this.speed * dt;
			this.y += this.fwd.y * this.speed * dt;
		}
		
	}
	
	//creates enemy bullet if it can shoot
	shoot() {
		if(this.fireShot) {
			return new EnemyBullet(0xFFAA0F,this.x,this.y);
		}
		else {
			return null;
		}
	}
	
	//bounce off walls
	reflectX(){
		this.fwd.x *= -1;
	}
	
	//bounce off walls
	reflectY(){
		this.fwd.y *= -1;
	}
}

//creates a player bullet
class Bullet extends PIXI.Graphics{
	constructor(color=0xFFFFFF, x=0, y=0,bulletx = 0, bullety = -1){
		super();
		this.beginFill(color);
		this.drawRect(-2,-3,4,6);
		this.endFill();
		this.x =x;
		this.y =y;
		
		this.fwd = {x:bulletx ,y:bullety};
		this.speed = 400;
		this.isAlive = true;
		Object.seal(this);
	}
	
	//moves the bullet
	move(dt=1/60){
		this.x += this.fwd.x * this.speed * dt;
		this.y += this.fwd.y * this.speed * dt;
	}
}

//creates enemy bullet
class EnemyBullet extends PIXI.Graphics{
	constructor(color=0xFFFFFF, x=0, y=0,bulletx = 0, bullety = -1){
		super();
		this.beginFill(color);
		this.drawRect(-2,-3,4,6);
		this.endFill();
		this.x =x;
		this.y =y;
		
		this.fwd = {x:0 ,y:1};
		this.speed = 400;
		this.isAlive = true;
		Object.seal(this);
	}
	
	move(dt=1/60){
		this.x += this.fwd.x * this.speed * dt;
		this.y += this.fwd.y * this.speed * dt;
	}
}

//creates the backgrounds of the sceens. 
class DrawBackground extends PIXI.Graphics{
	constructor(levelScene = false, endeLevel = false,color=0x808080, x=0, y=0,){
		//normal level sceen
		if(levelScene){
			super();
			
			this.beginFill(0x5A5A5A);
			this.drawRect(0,50,600,600);
			this.endFill();
			
			this.beginFill(color);
			this.drawRect(0,50,40,600);
			this.endFill();
			
			this.beginFill(color);
			this.drawRect(560,50,40,600);
			this.endFill();
			
			this.beginFill(color);
			this.drawRect(0,50,600,40);
			this.endFill();
			
			this.beginFill(color);
			this.drawRect(0,610,600,40);
			this.endFill();
			
			this.beginFill(0x964B00);
			this.drawRect(200,610,200,40);
			this.endFill();
			
			this.beginFill(0x964B00);
			this.drawRect(200,50,200,40);
			this.endFill();
			
			this.x =x;
			this.y =y;
		}
		//last level sceen
		else if(endeLevel){
			super();
			
			this.beginFill(0x3BB143);
			this.drawRect(0,50,600,600);
			this.endFill();
			
			this.beginFill(0x0000FF);
			this.drawRect(0,200,600,120);
			this.endFill();
			
			this.beginFill(0xD2B48C);
			this.drawRect(200,50,200,600);
			this.endFill();
			
			this.beginFill(color);
			this.drawRect(0,610,600,40);
			this.endFill();
			
			this.beginFill(0x964B00);
			this.drawRect(200,610,200,40);
			this.endFill();
		}
		//start and end game sceen
		else {
			super();
			this.beginFill(color);
			this.drawRect(0,0,600,650);
			this.endFill();
			this.x =x;
			this.y =y;
		}
		
		
	}
}
