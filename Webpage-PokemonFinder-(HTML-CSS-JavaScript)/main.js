 // 1
	
	//load button when the window loads.
	window.onload = loadButtons;
	
	//declare local storage variables
	const prefix = "jnt6801-";
	const indexKey = prefix + "index"
	const storedIndex = localStorage.getItem(indexKey);
	
	//declares the search type variable
	let currentSearchType = "";
	
	//console.log(storedIndex);
	
	
	//loads all buttons and local storage.
	function loadButtons(){ 
		document.querySelector("#item").onclick = function() {currentSearchType = "item"; searchButtonClicked()};
		document.querySelector("#search").onclick = function() {currentSearchType = "pokemon"; searchButtonClicked();};
		document.querySelector("#moves").onclick = function() {currentSearchType = "move"; searchButtonClicked()};
		document.querySelector("#next").onclick = nextThing;
		document.querySelector("#last").onclick = previousThing;
		if (storedIndex != null){
			document.querySelector("#searchterm").value = storedIndex;
		}
	}
	
	//create display term variable
	let displayTerm = "";
	
	//moves to the next index and displayes the item. 
	function nextThing(){
		currentTerm = document.querySelector("#searchterm").value;
		currentTerm = parseInt(currentTerm); 
		document.querySelector("#searchterm").value = 1 + currentTerm;
		searchButtonClicked();
	}
	//moves to the previous index and displayes the item. 
	function previousThing(){
		currentTerm = document.querySelector("#searchterm").value;
		currentTerm = parseInt(currentTerm); 
		document.querySelector("#searchterm").value = currentTerm - 1;
		searchButtonClicked();
	}
	
	//gets the url setup.
	function searchButtonClicked(){
		//console.log("searchButtonClicked() called");
		
		const POKE_URL = "https://pokeapi.co/api/v2/" + currentSearchType + "/";
		
		let term = document.querySelector("#searchterm").value;
		displayTerm = term;
		
		term = term.trim();
		
		term = encodeURIComponent(term);
		
		if(term.length < 1) return;
		
		let url = POKE_URL + term + "/";
		
		document.querySelector("#status").innerHTML = "<b>searching for "+currentSearchType+ ": " + displayTerm + "</b>";
		
		//console.log(url);
		
		getData(url,term);
		
	}
	
	//gets the data if it does not break.
	function getData(url,term){

		let xhr = new XMLHttpRequest();
		
		xhr.onload = catch404Error;
		
		xhr.onerror = dataError;
		
		try{		
			xhr.open("GET",url);
			xhr.send();
		}
		catch(error){
			//console.log("there was an error");
			document.querySelector("#content").innerHTML = `<p>"Search term does not exist or number is out of range."</p>`;
		}
	}
	
	//This is called on load to save the page from requesting info from pages that don't exist. 
	function catch404Error(e) {
		let xhr = e.target;
		
		//console.log(xhr.responseText);
		
		let obj;
		
		try{
			obj = JSON.parse(xhr.responseText);
			dataLoaded(obj)
		}
		catch(error){
			//console.log("there was an error");
			document.querySelector("#content").innerHTML = `<p>"Search term does not exist or number is out of range."</p>`;
		}
		
	}
	
	function dataLoaded(e) {
		obj = e;
		
		//console.log(obj.name);
		//console.log(currentSearchType);

		if(currentSearchType == "pokemon"){
			let name = "Name: "+ obj.name;
			let height = "Height: "+ obj.height;
			let weight = "Weight: "+ obj.weight;
			
			document.querySelector("#searchterm").value = obj.id;
			
			let id = obj.id;
		
			let pokemonImage = loadImage(id);
			//console.log(pokemonImage);
		
			let types = obj.types;
		
			let typeLine = "Typing: ";
			for (let i = 0; i < types.length; i++){
				let type = types[i];
				typeLine += type.type.name + "   ";
				//console.log(type.type.name);
			}
		
			let line = `<div class='result'><img src='${pokemonImage}' title = '${name}' />`;
			line += `<span></span><p>${name}<br>${typeLine}<br>${height}<br>${weight}</p></div>`;

			document.querySelector("#content").innerHTML = line;
		}
		if(currentSearchType == "item"){
			//console.log(obj.name);
			
			document.querySelector("#searchterm").value = obj.id;
			
			let id = obj.id;
			
			let name = "Name: "+ obj.name;
		
			let pokemonImage = loadItemImage(obj.name);
			//console.log(pokemonImage);
		
			let types = obj.types;
		
			let typeLine = obj.effect_entries[0].short_effect;
			//console.log(obj.effect_entries);

			let line = `<div class='result'><img src='${pokemonImage}' title = '${name}' />`;
			line += `<span></span><p>${name}<br>${typeLine}</p></div>`;

			document.querySelector("#content").innerHTML = line;
		}
		if(currentSearchType == "move"){
			let name = "Name: "+ obj.name;
			let accuracy = "Accuracy: "+ obj.accuracy;
			let pp = "PP: " + obj.pp;
			let power = "Power: " + obj.power;
			let damageType = "Attack Type: " + obj.damage_class.name;
			let moveType = "Type: " + obj.type.name;
			let discription = obj.effect_entries[0].effect;

			let line = `<div class='result'><p>${name}<br>${moveType}<br>${damageType}<br>${power}<br>${accuracy}<br>${pp}<br>${discription} </div>`;

			document.querySelector("#content").innerHTML = line;
		}
		localStorage.setItem(indexKey, document.querySelector("#searchterm").value);
		
	}
	
	//loads the pokemon Image
	function loadImage(e){
		if("true" == document.querySelector("#shiny").value) {
			return "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/shiny/" + e +".png";
		}
        else {
			return "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/" + e +".png";
		}
        
	}
	//loads the Item Image
	function loadItemImage(e){
        return  "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/items/" + e +".png";
        
	}
	
	//catch data errors. 
	function dataError(e){
		//console.log("An error occurred");
	}
	