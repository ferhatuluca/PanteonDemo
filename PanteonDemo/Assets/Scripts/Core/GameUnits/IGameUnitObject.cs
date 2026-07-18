namespace Core.GameUnits
{
	// this is bridge between GameUnit component and actual game units like soldier and building
	public interface IGameUnitObject
	{
		bool IsAvailableForInteract();
		void Death();
	}
}