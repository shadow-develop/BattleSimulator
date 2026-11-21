### Core Combat Objects ###

**1. Character / Weapon**
- Character contains health, accuracy, evasion, and an equipped weapon.
- Weapon contains its type, damage type, and minimum/maximum damage.
- Weapon base damage must satisfy 1 <= minimum damage <= maximum damage <= 1,000.
- Character configurations are reusable between battles.

**2. BattleState**
- Maintains temporary information during combat.
- Tracks both fighters' current HP and the current round.
- Prevents the original character data from being modified during the simulator battle.

**3. AttackResult**
- Records one attack.
- Contains attacker ID, defender ID, outcome (Hit, Miss, or Dodge), base damage, final damage, and defender's remaining HP.
- Misses and dodges record zero base and final damage and leave defender HP unchanged.
- Does not calculate damage itself.

**4. RoundResult**
- Contains the round number and a list of AttackResult objects.
- Each round contains one or two sequential attacks.
- The second fighter attacks only if still alive.

**5. BattleResult**
- Contains the complete battle report.
- Stores fighter IDs, winner, outcome, ending reason, total rounds, final HP, and all RoundResult objects.
- Outcomes: Fighter A wins, Fighter B wins, or Draw.
- Contains between 1 and 15 rounds.
- Ending reasons: Death or 15-round limit.

**6. BattleService**
- Receives two characters and validates their combat configuration.
- Creates temporary battle state.
- Simulates up to 15 rounds with a randomly selected first attacker each round.
- Calculates hit chance, weapon damage, and damage-type modifiers.
- Updates temporary HP and records every attack.
- Ends combat when one fighter reaches 0 HP or after round 15.
- Death takes precedence over the round limit, including during round 15.
- At the round limit, compares unrounded remaining HP percentages; equal percentages result in a draw.
- Returns a complete BattleResult.

### Damage-Type Rules

- Compare the damage types of the attacker's and defender's equipped weapons.
- Piercing beats Slashing, Slashing beats Blunt, and Blunt beats Piercing.
- Matching types deal 100% of base damage.
- An advantaged attack deals 125% of base damage, rounded upward to the next whole integer when fractional.
- A disadvantaged attack deals 75% of base damage, rounded downward to the previous whole integer when fractional.
- Roll base damage within the weapon's inclusive minimum and maximum bounds only after an attack hits.
- Apply the modifier to that roll to obtain whole-integer final damage, then reduce battle HP without going below zero.
- The 1,000 base-damage cap allows up to 1,250 final damage with advantage.

### Architecture Principles

- The BattleService contains combat logic, not MVC controllers.
- Result classes primarily store data rather than execute combat calculations.
- No database is required for V1.
- A separate Battle database entity may be introduced later to persist battle history.
- JavaScript can display or replay the completed battle progressively without running the combat simulation itself.
