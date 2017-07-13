using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum GameMode {
	preGame,
	loading,
	makeLevel,
	levelPrep,
	inLevel
}

public class WordGame : MonoBehaviour {
	static public WordGame S;

	public bool ________;

	public GameMode mode=GameMode.preGame;
	public WordLevel currLevel;

	void Awake () {
		S = this;
	}

	// Use this for initialization
	void Start () {
		mode = GameMode.loading;
		WordList.S.Init ();
	}

	public void WordListParseComplete() {
		mode = GameMode.makeLevel;
		currLevel = MakeWordLevel ();
	}

	public WordLevel MakeWordLevel(int levelNum=-1) {
		WordLevel level = new WordLevel ();
		if (levelNum == -1) {
			level.longWordIndex = Random.Range (0, WordList.S.longWordCount);
		} else {
		}
		level.levelNum = levelNum;
		level.word = WordList.S.GetLongWord (level.longWordIndex);
		level.charDict = WordLevel.MakeCharDict (level.word);
		StartCoroutine (FindSubWordsCoroutine (level));
		return (level);
	}

	public IEnumerator FindSubWordsCoroutine(WordLevel level) {
		level.subWords = new List<string> ();
		string str;
		List<string> words = WordList.S.GetWords ();
		for (int i = 0; i < WordList.S.wordCount; i++) {
			str = words [i];
			if (WordLevel.CheckWordInLevel (str, level)) {
				level.subWords.Add (str);
			}
			if (i % WordList.S.numToParseBeforeYield == 0) {
				yield return null;
			}
		}
		level.subWords.Sort ();
		level.subWords = SortWordsByLength (level.subWords).ToList ();
		SubWordSearchComplete ();
	}

	public static IEnumerable<string> SortWordsByLength(IEnumerable<string> e) {
		var sorted = from s in e
		             orderby s.Length ascending
		             select s;
		return sorted;
	}

	public void SubWordSearchComplete() {
		mode = GameMode.levelPrep;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
