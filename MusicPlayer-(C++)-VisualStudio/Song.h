#pragma once
#include <iostream>
#include <string>
using namespace std;

class Song
{
	private:
		string name;
		string artist;
		string URL;
	public:
		Song(string songName, string songArtist, string songURL);

		string getSongName();
		string getSongArtist();
		string getSongURL();
		void DisplaySongInfo();



};

