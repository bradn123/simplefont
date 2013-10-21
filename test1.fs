s" grf.fs" included
( ------------------------------------------------------------ )

variable phase

: draw
  0 0 pixel
  height 0 do
    width 0 do
      phase @ i + over 0 + c!
      j over 1 + c!
      phase @ i + j * over 2 + c!
      255 over 3 + c!
      4 +
    loop
  loop
  drop
;

: test1
  640 480 window
  begin
    event dup expose-event = if
      begin
        draw
        1 phase +!
        flip
      again
    else
    then
    drop
  again
;
test1
