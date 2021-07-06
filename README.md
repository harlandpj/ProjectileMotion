# ProjectileMotion
Fairly simple function to calculate the x-Axis position of a projectile in flight (from an intial height above ground) to determine if it lands with the bounds of a playfield.


Just code in a standard console app, inputs are, 

1: the starting height above ground, 
2: A velocity vector (to get speed and launch angle),
3: width of the playfield, 
4: A height above ground, which when passed (by the projectile falling down to reach it) we return the current x-Axis position of the projectile, returning true
   if the projectile dropped to this height and was within the width of the playfield, false otherwise.
