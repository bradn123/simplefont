s" grf.fs" included
s" font.fs" included
s" gimple1.fs" included
( ------------------------------------------------------------ )
( Redirect gforth terminal to grf + font )

: grf-key
  flip
  begin
    wait
    event expose-event = if
      clear flip
    then
    event negate dup 1 > swap 127 < and if
      last-key
      exit
    then
  again
;

: terminal
  640 480 window

  gimple1 font !
  10 font-width !  20 font-height !  100 font-weight
  font-margin @ font-x !  font-height @ font-y !

  ['] font-type is type
  ['] font-emit is emit
  ['] grf-key is xkey

  quit
;
terminal
