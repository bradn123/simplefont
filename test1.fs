s" grf.fs" included
s" font.fs" included
s" gimple1.fs" included
( ------------------------------------------------------------ )

: draw
  0 0 pixel
  height 0 do
    width 0 do
      i 255 height 1- */ over 0 + c!
      i 255 height 1- */ over 1 + c!
      i 255 height 1- */ over 2 + c!
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
      \ draw
      clear
      100 100 300 100 300 300 quartic
      100 100 100 300 300 300 quartic
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
