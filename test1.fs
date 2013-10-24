s" grf.fs" included
s" font.fs" included
s" gimple1.fs" included
( ------------------------------------------------------------ )

variable x  33 x !

: test1
  640 480 window
  begin
    poll
    event expose-event = if
      clear
\      100 100 300 100 300 300 quartic
\      100 100 100 300 300 300 quartic
      flip
    then
    event 0= if
      \ move mouse
    else event timeout-event = if
      100 ms
    else event 1 = if
      clear
      gimple1 font !
      0 font-x !
      x @ dup 5 + swap do i char-draw loop
      flip
      1 x +!
    else
      \ event . mouse-x . mouse-y . last-keysym . last-key emit cr
    then then then
  again
;
test1
