public class QuestionRowModel{

	public int typeId;
	public string answer;
	public int levelId;
	public string definition;
	public string synonym1;
	public string synonym2;
	public string antonym1;
	public string antonym2;
	public string clues1;
	public string clues2;
	public string clues3;
	public string clues4;
	public int hasDefinition;
	public int hasSynonym;
	public int hasAntonym;
	public int hasClues;

	public QuestionRowModel(int typeId,string answer, int levelId,string definition, string synonym1,string synonym2,string antonym1,string antonym2
		,string clues1, string clues2, string clues3, string clues4, int hasDefinition, int hasSynonym, int hasAntonym, int hasClues){
		this.typeId = typeId;
		this.answer = answer;
		this.levelId = levelId;
		this.definition = definition;
		this.synonym1 = synonym1;
		this.synonym2 = synonym2;
		this.antonym1 = antonym1;
		this.antonym2 = antonym2;
		this.clues1 = clues1;
		this.clues2 = clues2;
		this.clues3 = clues3;
		this.clues4 = clues4;
		this.hasDefinition = hasDefinition;
		this.hasSynonym = hasSynonym;
		this.hasAntonym = hasAntonym;
		this.hasClues = hasClues;

	}

}
