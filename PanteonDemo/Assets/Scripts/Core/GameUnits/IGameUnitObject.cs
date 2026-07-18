namespace Core.GameUnits
{
	// this is bridge between GameUnit component and actual game units like soldier and building
	public interface IGameUnitObject
	{
		// this is used for checking OnTriggerEnter2D
		bool IsAvailableForInteract();
		void Death();
	}
}