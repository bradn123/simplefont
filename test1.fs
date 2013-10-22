s" grf.fs" included
( ------------------------------------------------------------ )

: draw
  0 0 pixel
  height 0 do
    width 0 do
      i over 0 + c!
      j over 1 + c!
      i j * over 2 + c!
      255 over 3 + c!
      4 +
    loop
  loop
  drop
;

: test1
  640 480 window
  begin
    wait
    event expose-event = if
      draw
      flip
    then
    event 0= if
      \ move mouse
    else
      event . mouse-x . mouse-y . last-keysym . ." |" last-key type ." |" cr
    then
  again
;
test1
