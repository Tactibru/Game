using System.IO; 
using UnityEngine;
using UnityEditor; 
using UnityEditor.Callbacks; 
using System.Collections;



public static class PostBuildTrigger 
{
	private static DirectoryInfo targetDirectory; 
	private static string buildName; 
	private static string buildDataDirectory; 
	private static DirectoryInfo projectParent; 
	
	//name of folder in project directory containing files for build
	private static string srcName = "Assets/Scripts/Behaviors"; 
	private static int fileCount; 
	private static int dirCount; 

	
	
	//processBuild Function. 
	/// <summary>
	/// Neal Valiant
	/// Raises the post process build event.
	/// </summary>
	///during the build process, this is used to get the directory 
	///we want. and the string of the folder we want to create. 
	///after this we copy the files we want from the srcName string. 
	/// <param name='target'>
	/// Target.
	/// </param>
	/// <param name='path'>
	/// Path.
	/// </param>
	[PostProcessBuild]
	public static void OnPostProcessBuild(BuildTarget target, string path)
	{
		Debug.Log("Post Processing Build"); 
		
		//get Required Paths
		projectParent = Directory.GetParent(Application.dataPath); 
		buildName = Path.GetFileNameWithoutExtension(path); 
		targetDirectory = Directory.GetParent(path); 
		char divider = Path.DirectorySeparatorChar; 
		string dataMarker = "_Data"; //used specifically for windows standalone
		buildDataDirectory = targetDirectory.FullName + divider + buildName + dataMarker + divider; 
		
		//Do certain things to a file here
		fileCount = 0; 
		dirCount = 0; 
		CopyAll(new DirectoryInfo(projectParent.ToString() + divider + srcName), new DirectoryInfo(buildDataDirectory)); 
		
		Debug.Log("Copied: " + fileCount + " file" + ((fileCount != 1)?"s":"")+ ", " + dirCount + " folder" +((dirCount != 1)?"s":"")); 
	
	}
	/// <summary>
	/// Copies all.
	/// Neal Valiant
	/// This is where we find the directory and actually copy the files over
	/// to access. /// </summary>
	/// <param name='source'>
	/// Source.
	/// </param>
	/// <param name='target'>
	/// Target.;
	/// </param>
	public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
	{
		//check if the target directory exists, if not, create it 
		if(Directory.Exists(target.FullName) == false)
		{
			dirCount++; 
			Directory.CreateDirectory(target.FullName); 
		}
		
		//copy each file into its new directory
		foreach(FileInfo fi in source.GetFiles())
		{
			fileCount++; 
			fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true); 
		}
		
		//copy each subdirectory using recursion
		foreach(DirectoryInfo diSourceSubDir in source.GetDirectories())
		{
			dirCount++; 
			DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name); 
			CopyAll(diSourceSubDir, nextTargetSubDir); 
		}
		
	}
	
}
