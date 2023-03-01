// Project 2, Music Play-List.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <fstream>
#include <string>
#include "Song.h"
#include <vector>

using namespace std;

int main()
{
    //creates vector list of songs
    vector <Song> Songs;

    //declare variables
    string songName;
    string songAuther;
    string songURL;
    string command;
    string userInput;

    //opens file
    ifstream myfile;
    myfile.open("my_master_playlist.txt");

    try { //make sure the file works
        if (myfile.is_open()) {
            for (int i = 0; i < 10; i++) {
                std::getline(myfile, userInput);
                songName = userInput;
                std::getline(myfile, userInput);
                songAuther = userInput;
                std::getline(myfile, userInput);
                songURL = userInput;
                Songs.push_back(Song(songName, songAuther, songURL));
            }

        }
        else {//throw exeption if file problem
            throw runtime_error("File did not open. Ending Program");
        }
    }
    //catch exception and display messege 
    catch (runtime_error& errorMessege) {
        cout << errorMessege.what() << endl << endl;
        return 0;
    }
    //closes file
    myfile.close();

    //reset user input to empty
    userInput = "";

    //untill user quits
    while (userInput != "q" && userInput != "Q") {
        
        cout << "Song Menu :" << endl;
        
        //displays each item of the list. 
        for (int i = 0; i < Songs.size(); i++) {
            cout << "   ( " << i + 1 << " ) : " << Songs.at(i).getSongName() << endl;
        }
        cout << "   ( Q ) : QUIT" << endl;

        cout << "Select => ";

        //get user input
        cin >> userInput;

        cout << endl << endl;
        
        //if q or Q then display messege and quit. 
        if (userInput == "q" || userInput == "Q") {
            cout << "Ending program" << endl;
            break;
        }
        //if song one
        else if (userInput == "1") {
            songName = Songs.at(0).getSongName();
            songAuther = Songs.at(0).getSongArtist();
            songURL = Songs.at(0).getSongURL();


            cout << "Playing : " << songName;
            cout << " by " << songAuther << endl;

            //Creates command line and call it
            command = "start chrome --window-size=700,650 " + songURL;
            const char* system_command = command.c_str();
            system(system_command);

            //close the window to continue
            cout << "\nClose the Browser Window to Continue, then..." << endl;
            system("pause");
            cin.ignore();

        }
        //if song 2
        else if (userInput == "2") {
            songName = Songs.at(1).getSongName();
            songAuther = Songs.at(1).getSongArtist();
            songURL = Songs.at(1).getSongURL();


            cout << "Playing : " << songName;
            cout << " by " << songAuther << endl;

            //Creates command line and call it
            command = "start chrome --window-size=700,650 " + songURL;
            const char* system_command = command.c_str();
            system(system_command);

            //close the window to continue
            cout << "\nClose the Browser Window to Continue, then..." << endl;
            system("pause");
            cin.ignore();

        }
        //if song 3
        else if (userInput == "3") {
            songName = Songs.at(2).getSongName();
            songAuther = Songs.at(2).getSongArtist();
            songURL = Songs.at(2).getSongURL();


            cout << "Playing : " << songName;
            cout << " by " << songAuther << endl;

            //Creates command line and call it
            command = "start chrome --window-size=700,650 " + songURL;
            const char* system_command = command.c_str();
            system(system_command);

            //close the window to continue
            cout << "\nClose the Browser Window to Continue, then..." << endl;
            system("pause");
            cin.ignore();

        }
        //if song 4
        else if (userInput == "4") {
            songName = Songs.at(3).getSongName();
            songAuther = Songs.at(3).getSongArtist();
            songURL = Songs.at(3).getSongURL();


            cout << "Playing : " << songName;
            cout << " by " << songAuther << endl;

            //Creates command line and call it
            command = "start chrome --window-size=700,650 " + songURL;
            const char* system_command = command.c_str();
            system(system_command);

            //close the window to continue
            cout << "\nClose the Browser Window to Continue, then..." << endl;
            system("pause");
            cin.ignore();

        }
        //if song 5
        else if (userInput == "5") {
            songName = Songs.at(4).getSongName();
            songAuther = Songs.at(4).getSongArtist();
            songURL = Songs.at(4).getSongURL();


            cout << "Playing : " << songName;
            cout << " by " << songAuther << endl;

            //Creates command line and call it
            command = "start chrome --window-size=700,650 " + songURL;
            const char* system_command = command.c_str();
            system(system_command);

            //close the window to continue
            cout << "\nClose the Browser Window to Continue, then..." << endl;
            system("pause");
            cin.ignore();

        }
        //if song 6
        else if (userInput == "6") {
            songName = Songs.at(5).getSongName();
            songAuther = Songs.at(5).getSongArtist();
            songURL = Songs.at(5).getSongURL();


            cout << "Playing : " << songName;
            cout << " by " << songAuther << endl;

            //Creates command line and call it
            command = "start chrome --window-size=700,650 " + songURL;
            const char* system_command = command.c_str();
            system(system_command);

            //close the window to continue
            cout << "\nClose the Browser Window to Continue, then..." << endl;
            system("pause");
            cin.ignore();

        }
        //if song 7
        else if (userInput == "7") {
            songName = Songs.at(6).getSongName();
            songAuther = Songs.at(6).getSongArtist();
            songURL = Songs.at(6).getSongURL();


            cout << "Playing : " << songName;
            cout << " by " << songAuther << endl;

            //Creates command line and call it
            command = "start chrome --window-size=700,650 " + songURL;
            const char* system_command = command.c_str();
            system(system_command);

            //close the window to continue
            cout << "\nClose the Browser Window to Continue, then..." << endl;
            system("pause");
            cin.ignore();

        }
        //if song 8
        else if (userInput == "8") {
            songName = Songs.at(7).getSongName();
            songAuther = Songs.at(7).getSongArtist();
            songURL = Songs.at(7).getSongURL();


            cout << "Playing : " << songName;
            cout << " by " << songAuther << endl;

            //Creates command line and call it
            command = "start chrome --window-size=700,650 " + songURL;
            const char* system_command = command.c_str();
            system(system_command);

            //close the window to continue
            cout << "\nClose the Browser Window to Continue, then..." << endl;
            system("pause");
            cin.ignore();

        }
        //if song 9
        else if (userInput == "9") {
            songName = Songs.at(8).getSongName();
            songAuther = Songs.at(8).getSongArtist();
            songURL = Songs.at(8).getSongURL();


            cout << "Playing : " << songName;
            cout << " by " << songAuther << endl;

            //Creates command line and call it
            command = "start chrome --window-size=700,650 " + songURL;
            const char* system_command = command.c_str();
            system(system_command);

            //close the window to continue
            cout << "\nClose the Browser Window to Continue, then..." << endl;
            system("pause");
            cin.ignore();

        }
        //if song 10
        else if (userInput == "10") {
            songName = Songs.at(9).getSongName();
            songAuther = Songs.at(9).getSongArtist();
            songURL = Songs.at(9).getSongURL();


            cout << "Playing : " << songName;
            cout << " by " << songAuther << endl;

            //Creates command line and call it
            command = "start chrome --window-size=700,650 " + songURL;
            const char* system_command = command.c_str();
            system(system_command);

            //close the window to continue
            cout << "\nClose the Browser Window to Continue, then..." << endl;
            system("pause");
            cin.ignore();

        }
        //if invalid input send messege
        else {
            cout << "Invalid Input" << endl;
        }

        cout << endl << endl;
    }



    //Code from first part A:


    //string song_Name = "Carol of the Bells";
    //string song_Artist = "Trans-Siberian Orchestra";
    //string song_URL = "https://www.youtube.com/watch?v=sCabI3MdV9g&t=51s";

    //string command;

    //cout << "Playing : " << song_Name;
    //cout << " by " << song_Artist << endl;

    //Creates command line and call it
    //command = "start chrome --window-size=700,650 " + song_URL;
    //const char* system_command = command.c_str();
    //system(system_command);

    //close the window to continue
    //cout << "\nClose the Browser Window to Continue, then..." << endl;
    //system("pause");
    //cin.ignore();

    //cout << "Bye, Thanks For Playing" << endl;

    return 0;

}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
