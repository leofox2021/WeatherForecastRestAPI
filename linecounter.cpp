#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <chrono>
#include <filesystem>

using namespace std;
using namespace std::filesystem;

// Replace this with your own filename if you need to.
const string LinecounterFilename = "LineCount.txt";
// Put desired extensions here in following format: ".cs", ".py"
const vector<string> SuitableExtensions = { ".cs", ".cpp", ".json" };
// Put desired ignored directory names here in the following format "dir/subdir"
const vector<string> IgnoredDirectories = { "bin", "obj" }; 

time_t GetDateTime() 
{
    auto currentDate = chrono::system_clock::now();
    return chrono::system_clock::to_time_t(currentDate);
}

// Reads through the file and sums its lines
int ReadFile(string filePath)
{
    fstream file;
    file.open(filePath, ios::in);

    int lines = 0;
    
    if (file.is_open())
    {
        string line;
        while (getline(file, line))
            lines++;
    }

    file.close();
    return lines;
}

void WriteToFile(int lineCount, int filesOverall)
{
    fstream file;
    auto currentTime = GetDateTime();
    file.open(LinecounterFilename, ios::out);

    if (file.is_open())
    {
        file << "Just a total amount of code lines in the project to see how cool I am :)\n\n";
        file << "Files: " << filesOverall << "\n";
        file << "Line count: " << lineCount << "\n";
        file << "Parsed on: " << ctime(&currentTime) << "\n";
        file << "Bulild command on windows: "  << "g++ -o linecounter.exe linecounter.cpp\n";
        file << "Bulild command on linux: " << "g++ -o linecounter linecounter.cpp";
    }

    file.close();
}

void PrintResult(int lineCount, int filesOverall) 
{
    auto currentTime = GetDateTime();

    cout << "Files: " << filesOverall << "\n";
    cout << "Line count: " << lineCount << "\n";
    cout << "Parsed on: " << ctime(&currentTime);
}

vector<string> GetDirectoryFiles()
{
    vector<path> filesWithExtension;
    vector<string> filesWithoutIgnoredDirectories;

    for (auto &file : recursive_directory_iterator(current_path())) 
    {
        path filePath = file.path();

        for (auto &suitableExtension : SuitableExtensions) 
        {
            if (filePath.extension() == suitableExtension)
                filesWithExtension.push_back(file.path());
        }
    }

    for (auto &file : filesWithExtension) 
    {
        string filePathString = file.u8string();
        
        bool isInIgnoredDirectory = false;

        for (auto ignoredDirectory : IgnoredDirectories) 
        {
            if (filePathString.find(ignoredDirectory) != string::npos) 
            {
                isInIgnoredDirectory = true;
                break;
            }
        }

        if (!isInIgnoredDirectory)
            filesWithoutIgnoredDirectories.push_back(filePathString);
    }

    return filesWithoutIgnoredDirectories;
}

void Parse()
{
    int linesOverall = 0;
    int filesOverall = 0;

    for (string file : GetDirectoryFiles())
    {
        int linesInFile = ReadFile(file);
        linesOverall += linesInFile;
        filesOverall ++;
        cout << "Reading file: " << file << ", Lines: " << linesInFile << endl;
    }
    
    PrintResult(linesOverall, filesOverall);
    WriteToFile(linesOverall, filesOverall);
}

int main()
{
    Parse();
}