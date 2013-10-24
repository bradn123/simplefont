s" grf.fs" included
s" font.fs" included
s" gimple1.fs" included
( ------------------------------------------------------------ )

: draw
  clear
  gimple1 font !
  30 font-width !  60 font-height !
  8 0 do
    10 font-x !
    40 font-height @ i * + font-y !
    32 0 do j 32 * i + char-draw loop
  loop
  flip
;

: test1
  640 480 window
  begin
    poll
    event expose-event = if
      draw
    then
    event 0= if
      \ move mouse
    else event timeout-event = if
      100 ms
    else event 1 = if
    else
      \ event . mouse-x . mouse-y . last-keysym . last-key emit cr
    then then then
  again
;
test1
