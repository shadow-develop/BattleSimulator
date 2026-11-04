### Core Combat Objects ###

**1. Character / Weapon**
- Character contains health, accuracy, evasion, and an equipped weapon.
- Weapon contains its type, damage type, and minimum/maximum damage.
- Character configurations are reusable between battles.

**2. BattleState**
- Maintains temporary information during combat.
- Tracks both fighters' current HP and the current round.
- Prevents the original character data from being modified during the simulator battle.

**3. AttackResult**
- Records one attack.
- Contains attacker ID, defender ID, hit/miss result, base damage, final damage, and defender's remaining HP.
- Does not calculate damage itself.

**4. RoundResult**
- Contains the round number and a list of AttackResult objects.
- Each round contains one or two sequential attacks.
- The second fighter attacks only if still alive.

**5. BattleResult**
- Contains the complete battle report.
- Stores fighter IDs, winner, outcome, ending reason, total rounds, final HP, and all RoundResult objects.
- Outcomes: Fighter A wins, Fighter B wins, or Draw.
- Ending reasons: Death or 50-round limit.

**6. BattleService**
- Receives two characters and validates their combat configuration.
- Creates temporary battle state.
- Simulates up to 50 rounds with a randomly selected first attacker each round.
- Calculates hit chance, weapon damage, and damage-type modifiers.
- Updates temporary HP and records every attack.
- Ends combat when one fighter reaches 0 HP or after round 50.
- At the round limit, compares remaining HP percentages; equal percentages result in a draw.
- Returns a complete BattleResult.

### Architecture Principles

- The BattleService contains combat logic, not MVC controllers.
- Result classes primarily store data rather than execute combat calculations.
- No database is required for V1.
- A separate Battle database entity may be introduced later to persist battle history.
- JavaScript can display or replay the completed battle progressively without running the combat simulation itself.