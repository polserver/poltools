create_debug_context();
    
    // get a process scripting object by PID
getprocess( pid );

    // getpid: get this script's pid
getpid();

    //
    // unload_scripts: unload scripts from the script cache (they will be 
    //                 reloaded from disk on demand) currently running 
    //                 scripts will continue as normal.
    //                 Passing "" will unload all scripts.
    //
unload_scripts( scriptname := "" );

    //
    // set_script_option: Set script options
    //
set_script_option( optnum, optval );
const SCRIPTOPT_NO_INTERRUPT := 1;      // if 1, script runs until it sleeps
const SCRIPTOPT_DEBUG        := 2;      // if 1, prints any debug info included
const SCRIPTOPT_NO_RUNAWAY   := 3;      // if 1, doesn't warn about runaway conditions
const SCRIPTOPT_CAN_ACCESS_OFFLINE_MOBILES := 4;
    //
    // set_script_option(SCRIPTOPT_NO_INTERRUPT,1) is the same as set_critical(1)
    // set_script_option(SCRIPTOPT_DEBUG,1) is the same as set_debug(1)
    //

sleep( num_seconds );
sleepms( num_milliseconds );

    //
    // wait_for_event: sleep for a number of seconds until an event shows up
    //                 if timeout is 0, return immediately
    //                 returns 0 if no event was ready
    //
wait_for_event( num_seconds_timeout );

    //
    // events_waiting: the number of events waiting, 0+
    //
events_waiting();

    //
    // set_priority: the priority of a script is how many instructions it
    //               executes before switching to another script.
    //               default script priority is 1.
    //               priority range is 1 to 255.
    //               Returns previous priority.
set_priority( priority );

    //
    // set_critical: critical scripts run if they are not blocked, without
    //               interruption.  An infinite loop in a critical script
    //               will hang the server
    //
set_critical( critical );


    //
    // set_debug(debug): if debug=1, and the script was compiled with
    //                   'ecompile -i [script].src', each script source line
    //                   will be printed as it is executed.
    //                   if debug=0, disables this output.
    //
set_debug( debug );


start_script( script_name, param := 0 );
run_script_to_completion( script_name, param := 0 );


    // 
    // syslog(text): write text to the console, and to the log file
    //               includes context (calling script name)
    //
syslog( text );


    //
    // system_rpm(): returns the system RPM, which is the number of
    //               "game loop rotations" completed in the last minute.
    //               This can be zero!
    //
system_rpm();

	//
	// clear_event_queue(): Empties the event queue of the current script.
	//
clear_event_queue();

	//
	// set_event_queue_size(size): Sets the event queue size of the current script (default 20)
	//
set_event_queue_size(size);

    //
    // is_critical(): returns 1 if the calling script is set critical, else 0.
    //
is_critical();
