/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2008-07-26
*/
var poll_questions := {

    {"What is your gender?", {"Male", "Female", "Prefer not to answer"}},
    {"Approximately what is your age?", {"14 or under", "15 to 19", "20 to 29", "30 to 39", "40 to 49", "50 to 59", "60 or above"}},
    {"What sort of internet connection do you normally use to play on Fantasia?", {"Dial-up", "ISDN", "Cable Modem", "DSL", "Public connection", "I don't know"}},
    {"Have you ever played Ultima Online anywhere else other than Fantasia?", {"No, FS is my first experience", "Yes, I played on OSI", "Yes, I've played on other", "I've played private and OSI"}},
    {"How long have you been playing on Fantasia?", {"Less than a week", "Less than 1 month", "one to six months", "six months to a year", "from the beginning"}},
    {"In general, how would you rate Fantasia?", {"Excellent", "Pretty good", "Average", "Below average", "Awful"}},
    {"Where were you the evening of September 27th, 1974?", {"I don't remember", "I wasn't even born yet!", "I was drunk", "I was in Cleveland", "I was drunk in Cleveland"}},
    {"What is the main reason that you play on Fantasia?", {"My friends play here", "Because I enjoy adventuring", "Because I enjoy crafting", "I don't play here.", "Other"}},
    {"Overall, how do you rate the difficulty level of Fantasia?", {"Its much too difficult", "Its slightly too difficult", "Its about right", "Its slightly too easy", "Its much too easy"}},
    {"On average, how often do you think that quests should occur on Fantasia?", {"Once per week", "2 or 3 per week", "4 or 5 per week", "One every day"}},
    {"Change is an inevitable part of an online game.  How much change do you think should happen?", {"As little as possible", "Occasional tweaks and additions", "Additions and occasionally", "Change is a good thing"}},
    {"When you play on FS, about what percentage of the time do you spend hunting? (as opposed to crafting, talking, etc.)", {"Less than 10%", "10%", "25%", "50%", "75%", "More than 90%"}},
    {"When you play on FS, about what percentage of the time do you spend crafting? (as opposed to hunting, talking, etc.)", {"Less than 10%", "10%", "25%", "50%", "75%", "More than 90%"}},
    {"Regarding the difficulty of gaining stats only (stats are Strength, Intelligence and Dexterity), do you think that it is:", {"Much too easy", "Slightly too easy", "About right", "Slightly too hard", "Much too hard", "Its been a while"}},
    {"Each account has 5 character slots.  Out of those 5 characters (or potential characters), how many do you play regularly?", {"1", "2", "3", "4", "All 5"}},
    {"Have you ever participated in a seer-run quest?", {"Yes, several times", "Yes, a few times", "Yes, once or twice", "No, never"}},
    {"In the future, do you WANT to participate in seer-run quests?", {"Yes, very much", "Somewhat", "If nothing else to do", "No, I dislike seer quests"}},
    {"Regarding the difficulty of gaining skills only, do you think that it is:", {"Much too difficult", "Slightly too difficult", "About right", "Slightly too easy", "Much too easy"}},
    {"What is your favorite type of character to play?", {"Pure warrior", "Pure mage", "Pure ranger", "Pure bard", "Pure thief", "Pure crafter", "Pure power player"}},
    {"Regarding the number of magic items that spawn (and ignoring the quality of those that spawn):", {"Much too many", "Slightly too many", "About right", "Slightly too few", "Far too few"}},
    {"Things occasionally change in Fantasia.  What do you think of the amount of change that has occurs on average?", {"Too many changes too fast", "A few too many changes", "About right", "A few more changes", "Change is good"}},
    {"What is your opinion of PvP (player-vs-player) activites?  (tournaments, for example)", {"I hate them", "I dislike them", "I don't care", "I like them", "I love them"}},
    {"So, are you seeing anyone?", {"Yes (truth)", "Yes (lie)", "No (truth)", "No (lie)"}},
    {"When you go hunting, do you prefer to play by yourself or with others?", {"I always hunt solo", "Few people I may hunt with", "Several people I usually hunt with", "I usually hunt in a group"}},
    {"What do you think of macroing (using an external program to automatically perform tasks)", {"It should be against the rules", "As long as the player is there", "Its perfectly fine", "I don't know or care"}},
    {"In terms of material gain (gold, items, virtue, etc), how rewarding/profitable do you feel that hunting currently is?", {"Far too rewarding", "Somewhat too rewarding", "About right", "Slightly too low", "Far too low"}},
    {"Regarding the quality of magic items that you find (and ignoring the number that you find), would you say that the average magic quality is:", {"Much too high", "Slightly too high", "About right", "Slightly too low", "Much too low"}},
    {"Do you think that Fantasia is progressing in the right direction?", {"Yes", "Sorta", "Not really", "No, Heading the wrong way"}},
    {"If a shard-wipe were to occur, what would be your more likely reaction?", {"I would definitely quit", "I would probably quit", "I would think of quitting", "I would definitely stay"}},
    {"How many fingers am I holding up?", {"None", "One", "One - bad Drocket!", "Two", "Three", "Four", "Five", "All six of them"}},
    {"In order to add special items and graphics (pianos and fish tanks, for example), it requires all players to download and install special files.  What do you think of this process?", {"Its too hard/not worth the effort", "Don't care", "Piece of cake"}},
    {"Regarding the previously mentioned special files, how often should they be updated to add new items?", {"Too annoying to worry about", "Once or twice per year", "Maybe every 3-6 months", "Every 2 or 3 months", "Once per month"}},
    {"If you've ever required GM assistance/entered a help page, how would you rate the speed of the reply?  (if you've never needed GM help, just pick 'Not applicable')", {"Not applicable", "Very fast", "Pretty fast", "Not fast, not slow", "Somewhat slow", "I'm still waiting"}},
    {"Overall, how would you rate the difficult of the <b>mechanics</b> of the Fantasia (figuring out how to do things, understanding the spec system, the use of dot commands, and the like)", {"Very hard to understand", "Somewhat hard to understand", "About average", "Pretty easy to understand", "Very easy to understand"}},
    {"In terms of material gain (gold, items, virtue, etc), how rewarding/profitable do you feel that crafting currently is?", {"Far too rewarding", "Somewhat too rewarding", "About right", "Slightly too low", "Far too low"}},
    {"How much do you think that Fantasia should encourage/require roleplaying?", {"No encouragement is needed", "A bit of encouragement", "Strong encouragment", "Roleplaying should be required"}},
    {"How much do you think that Fantasia CURRENTLY encourages roleplaying?", {"Not at all", "Somewhat, but not enough", "About the right level", "Too much"}},
    {"Do you think *Edwards is cute?", {"Yes", "I hereby request deletion!"}},
    {"In terms of material gain, what activity do you think should be more rewarding?", {"Hunting a lot more profitable", "Hunting a bit more profitable", "They should be about equal", "Crafting a bit more profitable", "Crafting a lot more profitable"}},
    {"What do you think of systems designed to prevent players who play more from getting ahead of those who play less?  (Golden hour on OSI is a good example:  The first hour or so that you play each day, you gain skills quickly.  After that, gain drops off significantly so that those who play several hours per day don't get much more than those who play a little.)", {"They're a great idea", "They're an OK idea", "I don't care", "I somewhat dislike them", "I strongly dislike them"}},
    {"How much should Fantasia promote activities that require multiple players to complete (extremely strong dungeon that requires several players to clear, for example)", {"All activities should be soloable", "A few group-required activites", "Activities should be group-required"}},
    {"What do you think of Fantasia's limited system of travel (Recall spell has limited options)", {"I love it", "I like it", "Its about average", "I dislike it", "I hate it"}},
    {"What type of monsters do you prefer to fight?", {"A lot of weak monsters", "A few stronger monsters", "One very strong monster"}},
    {"Did you enjoy this poll?", {"Very much", "Sorta", "Not really", "Waste of time"}}
};
