# Claptrap-Robot
###ECEN 361 - Final Project, Spring 2016
###Mark Hall
###Pao Vang


##Objectives: Why did we build this robot?
ECEN 361 is an Embedded Systems class that teaches us the proper use of... you guessed it, embedded systems.  Now, we had the opportunity to create any project that we wanted to, from home automation systems, to creating motion tracking software, to robotics projects.  I definitely appreciate the fact that this class is so flexible in that aspect, because it allows us to be as creative as possible.  

I felt that the Claptrap project was a great one for several reasons:  

1. I had never built a robot before, and have wanted to do it since I was a child.  This was a perfect opportunity to fulfill a childhood dream, while getting credit for a class!
2. Claptrap is a hilarious robot, and he makes me laugh.  I was hoping that creating a project that has brought joy to so many people would be a project worthy of working on.
3. The internet is full of DIY projects.  However, after searching around, nobody had built a Claptrap robot that actually functions before.  This allowed me the opportunity to pioneer a new project, even though robotics is not a new field.

Ultimately we decided to use Windows IOT Core, a fairly new OS that has been created for the Raspberry Pi.  While some parts of the OS are still in development, we felt that the User Interface looked sleek and simple, and uploading our code to the Raspberry Pi through Visual Studio, once set up correctly, was very useful.  Furthermore, we were able to code the project in C#, a language that we are already familiar with.

##Equipment
Raspberry Pi 3
Arduino Uno
Battery Power pack
2 5V DC motor with wheels
4 servos
H-bridge chip
AdaFriut NeoPixel ring
Webcam
Trash can

##Design Overview
Windows IOT Core has the capability to upload apps onto the Raspberry Pi quite nicely.  We decided to utilize this feature, along with the ability to remotely connect with the Pi through any computer, tablet or phone with Windows 10 Operating System on it.  We created a wireless access point through cell phone tethering, though through any router or WiFi adapter works just as well.  From there, we load up a simple User Interface, with a camera, four slider bars for each servo, and directional commands for the DC wheels.  From that interface, commands are issued to the Pi remotely, which then connect to each function that we need to have it do.


##Procedure
1. The first thing that we did was gather the materials and ideas of functionality.  We started a list of materials needed for this project, and brainstorm of how we wanted claptrap to look like. Then, we had to decide whether or not Windows IOT Core was a viable option for the project; a few times, we nearly switched over to another Distro, but utimately we were able to make it work.

2. Getting the materials: we both ordered different materials.  Mark had previously obtained some materials already so we were able to use them.  Along the way, it was necessary to purchase more materials.
    
3. Testing of components
  * We first tested the DC motors and wheels to see if we were able to code them and make them run
     * We didn’t have all the materials to test the DC motors so we ordered some H-bridge chip to make sure that we don’t mess up the raspberry pi 3 that we were using, we also looked online for help on setting up a DC motor to the pi. 
     * We found out that we needed the H-bridge chip to make it work, the raspberry pi 3 was connected to the H-bridge chip then the chip connected to the DC motor
  * After the DC motors we tested the camera
     * We first tested Mark’s camera that he had ordered previously before we started the project but it wasn’t going as fast as we wanted it to be. 
     * We then went and got a cheaper lower grade web cam and it was a little bit better. We then went online to see the camera specs that the software we were using could use. It turned out that most of the cameras Windows IoT could handle were a bit lower grade as compare to the first camera we used. 
     * We decided to just use the lower grade camera. 
  * We then tested the servos for the arms
     * We started with Mark finding some examples online on how to set up the servos and on how they could be programed on Windows IoT which uses C# as its primary language. 
     * The example we had was pretty simple
     * From that example Mark was able to use it as a back up to programing the servos 
     * At first the servos were just on automatic movement with a timer controlling the movements
     * We talked about how we could do a button to control the servos but then ended up on using a slider as the controller. This meant that it would take a little more programing and calculations to get the servos to be where we needed them to be at, but over all it would be better on the User interface

*  The DC motore were quite simple to use, once we got the correct chip in place for us to create forward and backward motion.

*  IOT Core at this time only has basic PWM support.  Therefore, it was necessary for us to simulate a PWM through creating pulses around a 20 ms wait time.  Fortunately, a 20ms pulse is easily viable for the Raspberry Pi; the NeoPixel ring turned out to function on several hundred MHz, which was why we decided to use the Arduino as a standalone to power the device.

 4. Creating the robot body
  * We did not start out our project with a pre-created design.  Instead, we settled on creating one ourselves... for better or for worse.  This of course posed many challenges, but I feel that in the end we were able to have a very unique learning experience from the whole ordeal.  Once again, this was something that I had very little experience with, and we sort of learned as we went along.  I purchased a Dremel to create incisions, and it was possibly one of the best choices I could have made; the Dremel tools work perfectly with smaller parts such as robotics, and it definitely expedited the building process significantly.
  * For the outer shell design:
      * We started with a plastic 3lb trash can, then we drew out the parts that we wanted to cut out 
      * Mark cut out the pieces on the trash can and sprayed it with a silver spray paint as the first layer then clear as the second and finishing it off with yellow and white as the outer color to match the color of claptrap
      * Pao started trying to figure out how to put all the pieces together and how the wheels were going to go on the body
      * Pao started to cut different parts from old art paintings and wooden dials 
      * Those parts were then used as the inner skeleton structure to hold the wheels and all the parts together
      * We then hot glued and zip tied the all the pieces together, we also then decided to have different layers within claptrap so we could have it organized and be able to move things around and replace parts as needed
      * We put all the wires on the lower level and the main components on the top, this made it so that we can get to the raspberry pi easily and the battery pack




Video Links:
This videos is an explanation of our project done by Mark.

https://drive.google.com/open?id=0B_jdokOkHsH4d1R6Vy0ydkwzdHM

This video is to show the forward movement of claptrap.

https://drive.google.com/open?id=0B_jdokOkHsH4OUx1QTJOVkZzUlE

This video is to show the arm movement of claptrap.

https://drive.google.com/open?id=0B_jdokOkHsH4RHgzMlBUbjlva1U

This video shows the user interface on Mark’s desktop.

https://drive.google.com/open?id=0B_jdokOkHsH4QVNpVnBaOVdUZ0E




Conclusion: 
    


