using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestionSystemConst {

	public const int HINT_REMOVE_TIME = 3;
	public const int HINT_SHOW_LIMIT = 10;
	public const int HINT_PER = 0;
	public const int HINT_BUTTON_COOLDOWN = 2;
	public const int HINT_REMOVE_TYPE = 1;
	public const int HINT_SHOW_TYPE = 1;
	public const double HINT_TARGET_ASSOCIATION = 1.5;

	public const int ALLOW_SHOW_WORDCHOICE = 1;
	public const int ALLOW_SHOW_SELECTLETTER = 1;
	public const int ALLOW_SHOW_LETTERLINK = 0;
	public const int ALLOW_SHOW_TYPING = 1;
	public const int ALLOW_SHOW_SLOTMACHINE = 1;
	public const int ALLOW_SHOW_CHANGEORDER = 1;

	public const int ALLOW_REMOVE_WORDCHOICE = 0;
	public const int ALLOW_REMOVE_SELECTLETTER = 1;
	public const int ALLOW_REMOVE_LETTERLINK = 0;
	public const int ALLOW_REMOVE_TYPING = 1;
	public const int ALLOW_REMOVE_SLOTMACHINE = 1;
	public const int ALLOW_REMOVE_CHANGEORDER = 0;

	public const int ALLOW_SHUFFLE_SELECTLETTER = 1;

	public const double ANSWER_SPEED_BASETIME = 2.5;
	public const double ANSWER_SPEED_ASSOCIATION = 1;
	public const double ANSWER_SPEED_DEFINITION = 0.5;
	public const double ANSWER_SPEED_SELECTLETTER = 1;
	public const double ANSWER_SPEED_TYPING = 1.5;
	public const double ANSWER_SPEED_SLOTMACHINE = 1;
}
