#include "Song.h"
#include <iostream>

Song::Song(string songName, string songArtist, string songURL) {
	name = songName;
	artist = songArtist;
	URL = songURL;
}

string Song::getSongName() {
	return name;
}
string Song::getSongArtist() {
	return artist;
}
string Song::getSongURL() {
	return URL;
}
void Song::DisplaySongInfo() {
	cout << name << " by " << artist << endl;
}