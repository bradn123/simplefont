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
    i 50 * 3 + font-weight!
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
    event case
      expose-event of draw endof
      timeout-event of 100 ms endof
      event . mouse-x . mouse-y .
      last-keycode . last-keysym . last-key emit cr
    endcase
  again
;
test1
