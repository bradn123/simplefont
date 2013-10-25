s" grf.fs" included
s" font.fs" included
s" gimple1.fs" included
( ------------------------------------------------------------ )

: draw
  clear
  gimple1 font !
  10 font-width !  20 font-height !
  font-margin @ font-x !  font-height @ font-y !
  10 0 do
    i 50 * 3 + font-weight
    126 33 do i font-emit loop
    font-cr
    s" The quick brown fox jumped over the lazy dog." font-type
    font-cr
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
      event . mouse-x . mouse-y . last-keysym . last-key emit cr
    then then then
  again
;
test1
