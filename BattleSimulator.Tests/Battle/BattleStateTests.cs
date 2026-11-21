using BattleSimulator.Battle;
using BattleSimulator.Presets;

namespace BattleSimulator.Tests.Battle
{
    public class BattleStateTests
    {
        private BattleState _state = null!;

        [SetUp]
        public void SetUp()
        {
            var fighterA = CharacterPresets.CreateLeonidas();
            var fighterB = CharacterPresets.CreateHarald();
            _state = new BattleState(fighterA, fighterB);
        }

        [Test]
        public void ApplyDamageToFighterA_UpdatesBattleHealthOnly()
        {
            // Act
            _state.ApplyDamageToFighterA(30);

            // Assert
            Assert.That(_state.FighterACurrentHealth, Is.EqualTo(70));
            Assert.That(_state.FighterBCurrentHealth, Is.EqualTo(110));
            Assert.That(_state.FighterA.CurrentHealth, Is.EqualTo(100));
        }

        [Test]
        public void ApplyDamageToFighterB_UpdatesBattleHealthOnly()
        {
            // Act
            _state.ApplyDamageToFighterB(50);

            // Assert
            Assert.That(_state.FighterACurrentHealth, Is.EqualTo(100));
            Assert.That(_state.FighterBCurrentHealth, Is.EqualTo(60));
            Assert.That(_state.FighterB.CurrentHealth, Is.EqualTo(110));
        }

        [TestCase(0, 100)]
        [TestCase(100, 0)]
        [TestCase(150, 0)]
        public void ApplyDamageToFighterA_HandlesDamageBoundaries(int damage, int expectedHealth)
        {
            // Act
            _state.ApplyDamageToFighterA(damage);

            // Assert
            Assert.That(_state.FighterACurrentHealth, Is.EqualTo(expectedHealth));
            Assert.That(_state.FighterBCurrentHealth, Is.EqualTo(110));
            Assert.That(_state.FighterA.CurrentHealth, Is.EqualTo(100));
        }

        [TestCase(0, 110)]
        [TestCase(110, 0)]
        [TestCase(200, 0)]
        public void ApplyDamageToFighterB_HandlesDamageBoundaries(int damage, int expectedHealth)
        {
            // Act
            _state.ApplyDamageToFighterB(damage);

            // Assert
            Assert.That(_state.FighterBCurrentHealth, Is.EqualTo(expectedHealth));
            Assert.That(_state.FighterACurrentHealth, Is.EqualTo(100));
            Assert.That(_state.FighterB.CurrentHealth, Is.EqualTo(110));
        }

        [Test]
        public void Constructor_ThrowsWhenFighterAIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new BattleState(null!, _state.FighterB));

            Assert.That(ex!.ParamName, Is.EqualTo("fighterA"));
        }

        [Test]
        public void Constructor_ThrowsWhenFighterBIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new BattleState(_state.FighterA, null!));

            Assert.That(ex!.ParamName, Is.EqualTo("fighterB"));
        }

        [Test]
        public void ApplyDamageToFighterA_ThrowsExceptionForNegativeDamage()
        {
            // Act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _state.ApplyDamageToFighterA(-10));

            // Assert
            Assert.That(ex!.ParamName, Is.EqualTo("damage"));
            Assert.That(_state.FighterACurrentHealth, Is.EqualTo(100));
        }

        [Test]
        public void ApplyDamageToFighterB_ThrowsExceptionForNegativeDamage()
        {
            // Act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _state.ApplyDamageToFighterB(-10));

            // Assert
            Assert.That(ex!.ParamName, Is.EqualTo("damage"));
            Assert.That(_state.FighterBCurrentHealth, Is.EqualTo(110));
        }

        [TestCase(1, 2)]
        [TestCase(5, 6)]
        [TestCase(49, 50)]
        public void AdvanceRound_IncrementsByNumberOfCalls(int advances, int expectedRound)
        {
            // Act
            for (int i = 0; i < advances; i++)
            {
                _state.AdvanceRound();
            }

            // Assert
            Assert.That(_state.CurrentRound, Is.EqualTo(expectedRound));
        }
    }
}
