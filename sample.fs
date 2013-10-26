s" grf.fs" included
s" font.fs" included
s" gimple1.fs" included
( ------------------------------------------------------------ )

: draw
  clear
  gimple1 font !
  font-margin @ font-x !  font-height @ font-y !
  126 33 do i font-emit loop
  font-cr
  s" The quick brown fox jumped over the lazy dog." font-type
  font-cr
  flip
;

: handle-key
  last-key case
    [char] a of 1 font-width +! endof
    [char] z of -1 font-width +! endof
    [char] s of 1 font-height +! endof
    [char] x of -1 font-height +! endof
    [char] d of get-font-weight 10 + font-weight endof
    [char] c of get-font-weight 10 - font-weight endof
  endcase
  draw
;

: ui
  640 480 window
  10 font-width !  20 font-height !
  begin
    wait
    event case
      expose-event of draw endof
      press-event of handle-key endof
    endcase
  again
;
ui
