using UnityEngine;
using System.Collections;

public class SceneConversationBehavior : MonoBehaviour 
{
	public static SceneConversationBehavior instance;

	public TalkingEventManagerBehaviour.ConversationName introConversation;
	public TalkingEventManagerBehaviour.ConversationName[] battleQuips;
	public TalkingEventManagerBehaviour.ConversationName victoryConversation;
	public TalkingEventManagerBehaviour.ConversationName defeatConversation;

	void Start()
	{
		instance = this;
	}
}
