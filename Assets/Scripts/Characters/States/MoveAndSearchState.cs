namespace TowerDefence.Characters
{
    public class MoveAndSearchState : SearchState
    {
        public MoveAndSearchState(Character character): base(character) { }

        public override void Tick(float deltaTime)
        {
            Character.Agent.SetDestination(Character.FormationPosition);

            base.Tick(deltaTime);
        }

        public override ICharacterState Clone()
        {
            var clone = new MoveAndSearchState(Character);

            return clone;
        }
    }
}