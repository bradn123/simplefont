s" grf.fs" included
( ------------------------------------------------------------ )

: draw
  0 0 pixel
  height 0 do
    width 0 do
      i over 0 + c!
      i over 1 + c!
      i over 2 + c!
      255 over 3 + c!
      4 +
    loop
  loop
  drop
;

: test1
  640 480 window
  begin
    poll
    event expose-event = if
      draw
      flip
    then
    event 0= if
      \ move mouse
    else event timeout-event = if
      100 ms
    else
      event . mouse-x . mouse-y . last-keysym . last-key emit cr
    then then
  again
;
test1
