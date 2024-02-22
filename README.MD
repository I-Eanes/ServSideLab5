TL;DR
**This is a rundown of the major problems encountered and their solutions, scroll further down if you are interested in the play-by-play of this program's creation**
1. Accessing the API - After 20 minutes of looking through Microsoft Documentation, it was fixed.
2. Putting an object from the API into an object on the program fails - This could not be fixed, brought down the data as a string and just split it up as needed instead.
3. It takes to long to load each use of BreedGetter - moved the load-in of all breeds to the front, this creates a rough start-up time but all methods are very quick within the program.
4. I did decide to add the additional breed-search functionality - It created no issues minus a bit of standard troubleshooting and typo-fixing, this is just of note.


**I made the poor choice in judgement of starting this lab a little late, as such I was on a bit of a time crunch to finish it.
Nonetheless, here's how it went:**
- I spent 20 minutes trying to figure out just how to access the API.
- I failed to open GitHub because all attempts to log in just made the whole site *crash*, so I have decided to loop back to that part after everything else is done.
- Spent alot of time on Microsoft's tutorial documentations to figure out implementation.
- ASync does not work despite all NuGet packages being properly installed, will update this once its fixed.
- Loaded in the example code from Microsoft and it ran, still unsure what the issues are.
- I have discovered that by placing it back into a class together it does fix the Async issues, I am assuming it is because it needed to *NOT* run automatically on compilation.
- It succesfully connects to the API, but I cannot, for the life of me, figure out how to pull any actual information out of it.
- I have decided to scrap everything and start over.
**At this point a break was taken.**
- I have picked it up the next day, alot of scrapping had to be done at this point but I can now pull data from the API.
- It cannot be seperated into a printable string so im considering my options, I may just break off the text into what I need.
- I have gotten quite far in making the console return information but suddenly, the entire thing has just stopped loading, where before it would be handled in seconds, now it doesnt load at all.
- Update: I put a console.clear in the wrong spot, I am stupid.
- FACT GETTER WORKS, WE'RE IN BUSINESS.
- Though it is of note, rather than making the facts an object, its just a string that I have chopped everything *not a fact* off of. 
- If it works, it works.
**Moving on to the next functionality:**
- I started by bringing in all the info on my API's breeds section, it was a alot, but thankfully gave me an easy way to divide it up. (using "`{"data":`" as my delimeter)
- It was also at this point I created a Breeds class, so it would be easy to break everything up into it's pieces.
- I have just suddenly realized there are in fact 29 pages of dogs, I may have underestimated this one.
- Now is the point where I really started wishing I had figured out how to turn an API thingy into an object, splitting the attributes in string form is somehow so much harder than expected.
- After a bit of struggling all of the dog breeds have been turned into objects and theyre accessible from the console app. Woo!!
- This was a moment when I really debated how much more to do for this lab, and decided yea why not go all in.
**Bonus Round!**
- I want to create a functionality to allow users to search for desired doggy attributes so they can find the 'perfect' breed.
- Obviously personality traits are out the window since they are part of the description and *too* hard to seperate, but I can work with the rest!
- Very close to getting the Dog-Finder working, had some issues with attributes again where its just overcomplicated string but got it working eventually by just redo-ing the string -> lifespan conversion.
- Everything works, time for round two against Github and then I'll be 100% finished
**Day 3, despair continues**
- Almost done by now, just cleaning up comments and adding it to GitHub, due to the history of issues with Github i've decided to upload the code only when its 100% done.
- Cleanup is taking longer than expected though I've yet to break anything so thats a win.
- DONE WOO!!!