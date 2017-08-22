public class CharacterComputeModel {
	public bool isPlayer;
	public CharacterModel character;
	public float calculatedChar;

	public CharacterComputeModel(bool isPlayer, CharacterModel character, float calculatedChar){
		this.isPlayer = isPlayer;
		this.character = character;
		this.calculatedChar = calculatedChar;
	}
}
