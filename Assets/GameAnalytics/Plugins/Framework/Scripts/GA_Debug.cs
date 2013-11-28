/// <summary>
/// This class handles error and exception messages, and makes sure they are added to the Quality category 
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#if UNITY_METRO && !UNITY_EDITOR
using GA_Compatibility.Collections;
#endif

public class GA_Debug
{
	public bool SubmitErrors;
	public int MaxErrorCount;
	public bool SubmitErrorStackTrace;
	public bool SubmitErrorSystemInfo;
	
	private int _errorCount = 0;
	
	private bool _showLogOnGUI = false;
	public List<string> Messages;
	
	/// <summary>
	/// If SubmitErrors is enabled on the GA object this makes sure that any exceptions or errors are submitted to the GA server
	/// </summary>
	/// <param name="logString">
	/// The message <see cref="System.String"/>
	/// </param>
	/// <param name="stackTrace">
	/// The exception stack trace <see cref="System.String"/>
	/// </param>
	/// <param name="type">
	/// The type of the log message (we only submit errors and exceptions to the GA server) <see cref="LogType"/>
	/// </param>
	public void HandleLog(string logString, string stackTrace, LogType type)
	{
		//Only used if the GA_DebugGUI script is added to the GA object (for testing)
		if (_showLogOnGUI)
		{
			if (Messages == null)
			{
				Messages = new List<string>();
			}
			Messages.Add(logString);
		}
		
		//We only submit exceptions and errors
        if (SubmitErrors && _errorCount < MaxErrorCount && (type == LogType.Exception || type == LogType.Error))
		{
			// Might be worth looking into: http://www.doogal.co.uk/exception.php
			
			_errorCount++;
			
			bool errorSubmitted = false;
			
			string eventID = "Exception";
			
			if (SubmitErrorStackTrace)
			{
				SubmitError(eventID, logString.Replace('"', '\'').Replace('\n', ' ').Replace('\r', ' ') + " " + stackTrace.Replace('"', '\'').Replace('\n', ' ').Replace('\r', ' '));
				errorSubmitted = true;
			}
			
			if (SubmitErrorSystemInfo)
			{
				List<Hashtable> systemspecs = GA.API.GenericInfo.GetGenericInfo(logString);
			
				foreach (Hashtable spec in systemspecs)
				{
					GA_Queue.AddItem(spec, GA_Submit.CategoryType.GA_Log, false);
				}
				
				errorSubmitted = true;
			}
			
			if (!errorSubmitted)
			{
				SubmitError(eventID, null);
			}
		}
    }
	
	public void SubmitError(string eventName, string message)
	{
		Vector3 target = Vector3.zero;
		if (GA.SettingsGA.TrackTarget != null)
			target = GA.SettingsGA.TrackTarget.position;
		
		GA.API.Quality.NewErrorEvent(eventName, message, target.x, target.y, target.z);
	}
}
