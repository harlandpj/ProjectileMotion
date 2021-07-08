using System;
using System.Numerics; // for maths


namespace ProjectileMotion
{
    class ProjectileMotionCalc
    {
        // A simple implementation of a function to determine whether, given that a projectile is launched
        // above ground level (i.e. at height exceeding zero) with a given velocity (a vector), whether it will it drop down to (under gravity & negligible air resistance)
        // to a certain height, BEFORE exceeding the width of the playfield zone
        //
        // Returning true (and the xAxis postion of the projectile) if it has dropped to it while inside the given width,
        // false otherwise.

        // bool TryCalculateXPositionAtHeight()
        // ‘w’= width of playfield
        // ‘G’= acceleration due to gravity -9.8m/sec squared.
        // ‘p’= starting position of projectile.
        // ‘v’= initial velocity(this is a vector with both speed and direction components).
        // ‘h’= position (height) above x-axisthe it must drop down to, to give an xPosition result,
        //      note: this must be reached BEFORE the ball goes outside the (width of) playing area
        //  'w' width of playfield.

        // Function signature:bool TryCalculateXPositionAtHeight(float h, Vector2 p, Vector2 v,
        //                                                       float G, float w,ref float xPosition);

        // Feel free to use this function if it's of any use to you (a credit would be nice).
        static void Main(string[] args)
        {
            Console.WriteLine("Projectile Motion (at height) Calculator\n\n\n");

            Console.WriteLine("Take care when entering values, as there is NO input validation done here.\n\n\n");

            float playFieldWidth   = 0.0f; // width of playfield area
            float checkHeight      = 0.0f; // height (above ground) at which we do the check for success
            float gravity          = 9.8f; // gravity (normally -9.8m/s2, but allow for use elswehere e.g. in space)
            float xPositionLanding = new float(); // x Position when projectile landed

            Vector2 startPosition; // starting position
            Vector2 Velocity;      // initial velocity used to calculate Speed and launch angle

            bool bWithinPlayfield = true; // did projectile land within the width of the playfield
            string verbose; // full display of calculations or not.

            ProjectileMotionCalc theMotion = new ProjectileMotionCalc();

            // I will assume sensible values entered here, anyone implementating this code should add relevant checks for ranges (use Debug.Assert etc)
            // and handle any input errors
            Console.WriteLine("Enter Width of Play Field (>0.0):");
            playFieldWidth = Single.Parse(Console.ReadLine());

            Console.WriteLine("Enter \"Success\" Check Height (>0.0)");
            checkHeight = Single.Parse(Console.ReadLine());
            
            Console.WriteLine("Enter the Velocity Vector x Value:");
            Velocity.X = Single.Parse(Console.ReadLine());

            Console.WriteLine("Enter the Velocity Vector y Value:");
            Velocity.Y = Single.Parse(Console.ReadLine());

            Console.WriteLine("Enter the Starting X position:");
            startPosition.X = Single.Parse(Console.ReadLine());

            Console.WriteLine("Enter the Starting Y position:");
            startPosition.Y = Single.Parse(Console.ReadLine());

            Console.WriteLine("Enter value for Gravity (normally 9.8): ");
            gravity = Single.Parse(Console.ReadLine());

            Console.WriteLine("Do you want a verbose (display of all calculations) display? Enter Y or N");
            verbose = Console.ReadLine();

            Console.WriteLine("\n\nWidth: " + playFieldWidth);
            Console.WriteLine("Check Height: " + checkHeight);
            Console.WriteLine("Velocity Vector: (" + Velocity.X +"," + Velocity.Y+")");
            Console.WriteLine("Starting Position: (" + startPosition.X + "," + startPosition.Y + ")");
            Console.WriteLine("Gravity : -" + gravity + " metres per second squared");

            // now call the function to calculate
            
            bWithinPlayfield = theMotion.TryCalculateXPositionAtHeight(checkHeight, startPosition, Velocity, gravity, playFieldWidth, ref xPositionLanding, verbose);

            if (bWithinPlayfield) {
                Console.WriteLine("Success - Projectile landed within the playfield at x Position: " +xPositionLanding.ToString("0.000"));
            }
            else Console.WriteLine("Failure - Projectile landed outside the playfield at x Position: "+xPositionLanding.ToString("0.000"));
        }

        // function to calculate landing position of a projectile launched at a height above ground level
        bool TryCalculateXPositionAtHeight(float h, Vector2 p, Vector2 v, float G, float w, ref float xPosition, string Verbose)
        {
            float checkHeight = h;   // this is the h value for clarity of reading
            float currentX    = p.X; // set to starting X position initially
            float currentY    = p.Y; // set to starting Y position initially
            float currentTime = 0.0f; // set start time to zero

            // Get the launch angle, and convert from radians to degrees
            float launchAngle = (float)Math.Atan(v.Y/v.X) * 180 / (float)Math.PI;

            // Calculate overall starting velocity(speed), and the directional velocities(x and y axes)
            float velocityV  = (float)Math.Sqrt((v.X * v.X) + (v.Y * v.Y)); // starting Speed
            float velocityX  = velocityV * (float)Math.Cos(launchAngle);    // horizontal velocity
            float velocityY  = velocityV * (float)Math.Sin(launchAngle);    // vertical velocity
            
            // now get the total flight time for the projectile when launched (from a “height above ground”) to reach
            // the ground (i.e. height = 0)
            float timeToLand = (float)(velocityV * (float)Math.Sin(launchAngle) + Math.Sqrt(Math.Pow(velocityV * (Math.Sin(launchAngle)),2) + 2 * G * p.Y))/ G;

            // Now we will calculate the x & y positions(at the current time in the loop),do the height check, 
            // and exit the loop immediately IF we reach the required height
            // Note: I have assumed a time increment of 1ms (0.001) in loop - this may be too precise or not enough to give the exact
            // millimetre perfect X position, but the idea is correct - so modify for your own code!

            while (currentTime <= timeToLand)
            {
                currentX = velocityX * currentTime; //simple distance over time calculation - height is more complicated
                currentY = p.Y + (velocityY * currentTime) - G * (float)Math.Pow(currentTime,2)/2;
                
                if (currentY <= checkHeight)
                {
                    // we have fallen to the height to check, and still within playfield
                    xPosition = currentX; 
                }
                currentTime += 0.001f; // incrementby 1ms in flight time

            }

            if ((Verbose.CompareTo("Y") == 0)) 
            {
                Console.WriteLine("Launch     Angle is " + launchAngle.ToString("0.000") + " degrees.\n");
                Console.WriteLine("Starting Speed is " + velocityV.ToString("0.000") + " m/s.\n");
                Console.WriteLine("Horizontal Velocity component is " + velocityX.ToString("0.000") + " m/s.\n");
                Console.WriteLine("Vertical   Velocity component is " + velocityY.ToString("0.000") + " m/s.\n");
                Console.WriteLine("Time to land " + timeToLand.ToString("0.000") + " second(s).\n");
            }

            if (xPosition <= w)
            {
                // landed inside playfield
                return true;
            }
            else return false;
        }
    }
}
