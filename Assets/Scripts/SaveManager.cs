using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/** Manager for storing recorded user data into a CSV file 
  *  
  * 
  *
  */

public class SaveManager
{  
    /** Variable used to create the CSV folder and filename
      * First varibale is name of folder
      * Second variable is name of file followed by .<!-- type of save -->
      * Third variable is the report separator since CSV get columed by a comma
      * Lastly the for-loop generates the header for the csv report
      * the loop will run for as many strings / header you make it for
      */
    private static string reportDirectoryName = "Report";
    private static string reportFileName = "report.csv";
    private static string reportSeparator = ",";
    private static string[] reportHeaders = new string[7] 
    {
        "current section",
        "qubit state generated",
        "qubit state targeted",
        "question generated time stamp",
        "question component index",  
        "user answer",       
        "user answered time stamp"
    };
    private static string timeStampHeader = "general time stamp";

    // Verifies a directory and file before appending 
    // This function appends to the report, it adds another row 
    // to the report according to the lenght and separates the end of the 
    // string with a comma to properly set columnize it. 
    public static void AppendToReport(string[] strings) 
    {
      VerifyDirectory();
      VerifyFile();
      using(StreamWriter sw = File.AppendText(GetFilePath())) 
      {
        string finalString = "";
        for (int i = 0; i < strings.Length; i++)
        {
          if (finalString != "") 
          {
            finalString += reportSeparator;
          }
          finalString += strings[i];
        }
        finalString += reportSeparator + GetTimeStamp();
        sw.WriteLine(finalString);
      }
    }
    // This function verifies if a directory exists and creates the initial report
    public static void CreateReport()
    {
      VerifyDirectory();
      using(StreamWriter sw = File.CreateText(GetFilePath())) 
      {
        string finalString = "";
        for (int i = 0; i < reportHeaders.Length; i++)
        {
          if (finalString != "") 
          {
            finalString += reportSeparator;
          }
          finalString += reportHeaders[i];
        }
        finalString += reportSeparator + timeStampHeader;
        sw.WriteLine(finalString);
      }   
    }
    
    // This function verifies if a directory exists, if not it creates one
    static void VerifyDirectory() 
    {
      string dir = GetDirectoryPath();
      if (!Directory.Exists(dir)) 
      {
        Directory.CreateDirectory(dir);
      }
    }

    // This function verifies if a file exists, if not it creates one
    static void VerifyFile ()
    {
      string file = GetFilePath();
      if (!File.Exists(file))
      {
        CreateReport();
      }
    }

    // This gets the directory path from where the unity project is and 
    // establishes the path and report directory name
    static string GetDirectoryPath() 
    {
      return Application.dataPath + "/" + reportDirectoryName;
    }

    // This gets the directory path from where the unity project is and 
    // establishes the path and file name
    static string GetFilePath() 
    {
      return GetDirectoryPath()  + "/" + reportFileName;
    }

    // Gets the UTC system time and date and stringfies it
    static string GetTimeStamp()
    {
      return System.DateTime.UtcNow.ToString();
    }
}
