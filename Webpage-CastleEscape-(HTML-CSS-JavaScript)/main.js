// We will use `strict mode`, which helps us by having the browser catch many common JS mistakes
// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Strict_mode
"use strict";
const app = new PIXI.Application({
    width: 600,
    height: 650
});
document.body.appendChild(app.view);

// constants
const sceneWidth = app.view.width;
const sceneHeight = app.view.height;	

// pre-load the images
app.loader.
    add([
        "images/playerKnightOne.png",
    ]);
app.loader.onProgress.add(e => { console.log(`progress=${e.progress}`) });
app.loader.onComplete.add(setup);
app.loader.load();

// aliases
let stage;

// game variables
let startScene;
let gameScene,ship,scoreLabel,lifeLabel,levelLabel,shootSound,hitSound,fireballSound;
let gameOverScene;

let circles = [];
let bullets = [];
let enemyBullets = [];
let explosions = [];
let explosionTextures;
let score = 0;
let life = 100;
let levelNum = 1;
let paused = true;

let gameOverScoreLabel;


//new stuff
window.addEventListener("keyup", onKeyUp);
window.addEventListener("keydown", onKeyDown);

let wKey = false;
let sKey = false;
let aKey = false;
let dKey = false;
let bulletTimer = 0;

//sets up the game stages and needed assets
function setup() {
	stage = app.stage;
	// #1 - Create the `start` scene
	startScene = new PIXI.Container();
	stage.addChild(startScene);
	
	// #2 - Create the main `game` scene and make it invisible
	gameScene = new PIXI.Container();
	gameScene.visible = false;
	stage.addChild(gameScene);

	// #3 - Create the `gameOver` scene and make it invisible
	gameOverScene = new PIXI.Container();
	gameOverScene.visible = false;
	stage.addChild(gameOverScene);
	
	// #4 - Create labels for all 3 scenes
	createLablesAndButtons();
	
	// #5 - Create ship
	ship = new Ship();
	gameScene.addChild(ship);
	
	// #6 - Load Sounds
	shootSound = new Howl({
		src: ['sounds/sword-schwing-40520.mp3']
	});

	hitSound = new Howl({
		src: ['sounds/spawn-sound-43782.mp3']
	});

	// #8 - Start update loop
	app.ticker.add(gameLoop);

	// #9 - Start listening for click events on the canvas
	app.view.onclick = fireBullet;
	
	// Now our `startScene` is visible
	// Clicking the button calls startGame()
}

//creates the text and images on the game sceens. 
function createLablesAndButtons() {
	startScene.addChild(new DrawBackground());
	let buttonStyle = new PIXI.TextStyle({
		fill: 0x0000FF,
		fontSize: 48,
		fontFamily: "Futura"
	});
	
	// 1 - setup startScene 
	let startLabel1 = new PIXI.Text("Castle Escape");
	startLabel1.style = new PIXI.TextStyle({
		fill: 0x0000FF,
		fontSize: 96,
		fontFamily: "Futura",
		stroke: 0x000000,
		strokeThickness: 6
	});
	startLabel1.x = 30;
	startLabel1.y = 120;
	startScene.addChild(startLabel1);
	
	// 1B
	let startLabel2 = new PIXI.Text("Lets fight a way out!");
	startLabel2.style = new PIXI.TextStyle({
		fill: 0x0000FF,
		fontSize: 42,
		fontFamily: "Futura",
		fontStyle: "Bold",
		stroke: 0x000000,
		strokeThickness: 6
	});
	startLabel2.x = 100;
	startLabel2.y = 300;
	startScene.addChild(startLabel2);
	
	// 1C
	let startButton = new PIXI.Text("Start Game");
	startButton.style = buttonStyle;
	startButton.x = 175;
	startButton.y = sceneHeight - 200;
	startButton.interactive = true;
	startButton.buttonMode = true;
	startButton.on("pointerup", startGame);
	startButton.on('pointerover', e => e.target.alpha = 0.7);
	startButton.on('pointerout', e => e.currentTarget.alpha = 1.0);
	startScene.addChild(startButton);
	
	// 2 setup gameScene
	let textStyle = new PIXI.TextStyle({
		fill: 0x808080,
		fontSize: 20,
		fontFamily: "Futura",
		stroke: 0x0000FF,
		strokeThickness: 2
	});
	gameScene.addChild(new DrawBackground(true));
	//2A
	scoreLabel = new PIXI.Text();
	scoreLabel.style = textStyle;
	scoreLabel.x = 50;
	scoreLabel.y = 15;
	gameScene.addChild(scoreLabel);
	increaseScoreBy(0);
	
	//2B
	lifeLabel = new PIXI.Text();
	lifeLabel.style = textStyle;
	lifeLabel.x = 255;
	lifeLabel.y = 15;
	gameScene.addChild(lifeLabel);
	decreaseLifeBy(0);
	
	levelLabel = new PIXI.Text();
	levelLabel.style = textStyle;
	levelLabel.x = 505;
	levelLabel.y = 15;
	gameScene.addChild(levelLabel);
	increaseLevelBy();
	
	// 3 - set up `gameOverScene`
	// 3A - make game over text
	
	gameOverScene.addChild(new DrawBackground());
	
	let gameOverText = new PIXI.Text(`Game Over!`);
	textStyle = new PIXI.TextStyle({
		fill: 0x0000FF,
		fontSize: 64,
		fontFamily: "Futura",
		stroke: 0x000000,
		strokeThickness: 6
	});
	gameOverText.style = textStyle;
	gameOverText.x = 125;
	gameOverText.y = sceneHeight/2 - 160;
	gameOverScene.addChild(gameOverText);

	// 3B - make "play again?" button
	let playAgainButton = new PIXI.Text("Play Again?");
	playAgainButton.style = buttonStyle;
	playAgainButton.x = 150;
	playAgainButton.y = sceneHeight - 200;
	playAgainButton.interactive = true;
	playAgainButton.buttonMode = true;
	playAgainButton.on("pointerup",startGame); // startGame is a function reference
	playAgainButton.on('pointerover',e=>e.target.alpha = 0.7); // concise arrow function with no brackets
	playAgainButton.on('pointerout',e=>e.currentTarget.alpha = 1.0); // ditto
	gameOverScene.addChild(playAgainButton);
	
	gameOverScoreLabel = new PIXI.Text();
	gameOverScoreLabel.style = textStyle;
	gameOverScoreLabel.x = 50;
	gameOverScoreLabel.y = sceneHeight - 350;
	gameOverScene.addChild(gameOverScoreLabel);
	
}

//see if a movement key is down
function onKeyDown(event) {
	
	if(event.key === 'w'){
		wKey = true;
	}
	if(event.key === 'a'){
		aKey = true;
	}
	if(event.key === 's'){
		sKey = true;
	}
	if(event.key === 'd'){
		dKey = true;
	}
}

//see if a movement key is up
function onKeyUp(event) {
	if(event.key === 'w'){
		wKey = false;
	}
	if(event.key === 'a'){
		aKey = false;
	}
	if(event.key === 's'){
		sKey = false;
	}
	if(event.key === 'd'){
		dKey = false;
	}
}

//starts the game
function startGame(){
	startScene.visible = false;
	gameOverScene.visible = false;
	gameScene.visible = true;
	levelNum = 1;
	score = 0;
	life = 100;
	increaseScoreBy(0);
	decreaseLifeBy(0);
	ship.x = 300;
	ship.y = 600;
	loadLevel();
}

//increase display score
function increaseScoreBy(value) {
	score += value;
	scoreLabel.text = `Score ${score}`;
}

//decreases display life
function decreaseLifeBy(value){
	life -= value;
	life = parseInt(life);
	lifeLabel.text = `Life    ${life}%`;
}

//increases level count
function increaseLevelBy() {
	levelLabel.text = `Room ${levelNum}`;
}

//the gameplay loop
function gameLoop(){
	if (paused) return; // keep this commented out for now
	
	// #1 - Calculate "delta time"
	let dt = 1/app.ticker.FPS;
	if (dt > 1/12) dt=1/12;

	
	// #2 - Move Ship
	let mousePosition = app.renderer.plugins.interaction.mouse.global;
	//ship.position = mousePosition;
	
	let amt = 6 * dt;
	

	let newX = ship.x;
	let newY = ship.y;
	
	//move ship based on key down
	if(aKey){
		newX = lerp(ship.x, ship.x - 20, amt);
	}
	if(dKey){
		newX = lerp(ship.x, ship.x + 20, amt);
	}
		
	if(wKey){
		newY = lerp(ship.y, ship.y - 20, amt);
	}
	if(sKey){
		newY = lerp(ship.y, ship.y + 20, amt);
	}
	let h2 = ship.height/2;
	ship.y = clamp(newY,0+h2+90,sceneHeight-h2-40);
	let w2 = ship.width/2;
	ship.x = clamp(newX,0+w2+40,sceneWidth-w2-40);
	
	//Handel enemy bullet fire
	if(bulletTimer >= 100){
		for(let c of circles) {
			if(c.fireShot) {
				let newBullet = c.shoot();
				enemyBullets.push(newBullet);                         
				gameScene.addChild(newBullet); 
			}
		}
		bulletTimer = 0;
	}
	else {
		bulletTimer++;
	}
	
	
	// #3 - Move Circles
	for (let c of circles){
		let fullX = ship.x - c.x;
		let fullY = ship.y - c.y;
	
		let directionVector = Math.sqrt((fullY * fullY)+(fullX*fullX));
	
	
		let normalizedX = fullX / directionVector;
		let normalizedY = fullY / directionVector;
		
		c.move(dt, normalizedX, normalizedY);
		if (c.x - 40 <= c.radius || c.x + 40 >= sceneWidth - c.radius){
			c.reflectX();
			c.move(dt);
		}
		if(c.y - 90 <= c.radius || c.y + 40 >= sceneHeight-c.radius) {
			c.reflectY();
			c.move(dt);
		}
	}
	
	// #4 - Move Bullets
	for (let b of bullets){
		b.move(dt);
	}
	
	for (let b of enemyBullets){
		b.move(dt);
	}
	// #5 - Check for Collisions
	for(let c of circles) {
		for(let b of bullets){
			//5A
			if(rectsIntersect(c,b)){
				gameScene.removeChild(c);
				c.isAlive = false;
				gameScene.removeChild(b);
				b.isAlive = false;
				increaseScoreBy(1);
			}
		
			if(b.y < -10) b.isAlive = false;
		}
		
		//5B
		if(c.isAlive && rectsIntersect(c,ship)){
			hitSound.play();
			gameScene.removeChild(c);
			c.isAlive = false;
			decreaseLifeBy(20);
		}
		
	}
	//see if enemy bulets hit the player 
	for(let b of enemyBullets){
		if(rectsIntersect(b,ship)){
			hitSound.play();
			gameScene.removeChild(b);
			b.isAlive = false;
			decreaseLifeBy(20);
		}
		
		if(b.y < -10) b.isAlive = false;
	}
	
	// #6 - Now do some clean up
	bullets = bullets.filter(b=>b.isAlive);
	
	enemyBullets = enemyBullets.filter(b=>b.isAlive);
	
	circles = circles.filter(c=>c.isAlive);
	
	explosions = explosions.filter(e=>e.playing);
	                                             
	// #7 - Is game over?                        
	if (life <= 0 || levelNum > 11){
		end();
		return; // return here so we skip #8 below
	}
	                                             
	// #8 - Load next level                      
	if (circles.length == 0){
		//see if player is in correct zone 
		if(ship.x >= 200 && ship.x <= 400){
			if(ship.y <= 180){
				levelNum ++;
				increaseLevelBy();
				loadLevel();
			}
		}
	}
}
                                        
//creates the enemys                                                  
function createCircles(numCircles){              
	for(let i=0; i <numCircles;i++){
		//creats shooter enemys
		if(i % 5 == 0){
			let c = new Circle(9,0xFFAA0F, 0,0,false, true);
			c.x = Math.random() * (sceneWidth - 100) + 50;
			c.y = Math.random() * (sceneHeight - 400) + 100;
			circles.push(c);                         
			gameScene.addChild(c);
		}
		//creates enemys that chase the player
		else if(i % 2 == 1){
			let c = new Circle(10,0xFFFF00,0,0,true);  
			c.x = Math.random() * (sceneWidth - 110) + 55;
			c.y = Math.random() * (sceneHeight - 400) + 195;
			circles.push(c);                         
			gameScene.addChild(c); 			
		}
		//create the wandering enemys 
		else {
			let c = new Circle(7,0xFF00FF, 0,0,false); 
			c.x = Math.random() * (sceneWidth - 95) + 45;
			c.y = Math.random() * (sceneHeight - 400) + 95;
			circles.push(c);                         
			gameScene.addChild(c); 
		}
		                  
	}                                            
}                                                
//loads the next level                                                 
function loadLevel(){
	//if on excape level
	if(levelNum < 11){
		gameScene.addChild(new DrawBackground(true));
		gameScene.addChild(ship);
		createCircles(levelNum * 5);
	}
	//on normal level
	else{
		gameScene.addChild(new DrawBackground(false,true));
		gameScene.addChild(ship);
	}
	bulletTimer = 0;
	ship.x = 300;
	ship.y = 550;
	paused = false;                              
}                                                

//creates player bullet
function fireBullet(e){
	let rect = app.view.getBoundingClientRect();
	let mouseX = e.clientX - rect.x;
	let mouseY = e.clientY - rect.y;
	if(paused) return;
	
	//start finding needed vector of fire
	let fullX = mouseX - ship.x;
	let fullY = mouseY - ship.y;
	
	let directionVector = Math.sqrt((fullY * fullY)+(fullX*fullX));
	
	let normalizedX = fullX / directionVector;
	let normalizedY = fullY / directionVector;
	
	//create a new bullet with proper vector positions.
	let b = new Bullet(0xFFFFFF,ship.x,ship.y,normalizedX,normalizedY);
	
	bullets.push(b)
	gameScene.addChild(b);
	
	shootSound.play();
	
}

//loads end game sceen and clears lists. 
function end() {
	gameOverScoreLabel.text = `Your final score: ${score}`;
	paused = true;
	
	circles.forEach(c=>gameScene.removeChild(c));
	circles = [];
	
	bullets.forEach(b=>gameScene.removeChild(b));
	bullets = [];
	
	enemyBullets.forEach(b=>gameScene.removeChild(b));
	enemyBullets = [];
	
	explosions.forEach(e=>gameScene.removeChild(e));
	explosions = [];
	
	gameOverScene.visible = true;
	gameScene.visible = false;
}