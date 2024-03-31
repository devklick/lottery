import { Badge, Group, Overlay } from "@mantine/core";
import { useTimeout } from "@mantine/hooks";
import { useEffect } from "react";

interface TimedOverlayProps {
  duration: number;
  show: boolean;
  onTimeElapsed(): void;
}

function TimedOverlay({ show, duration, onTimeElapsed }: TimedOverlayProps) {
  const timeout = useTimeout(() => {
    onTimeElapsed();
  }, duration);

  useEffect(() => {
    if (show) timeout.start();
  }, [show]);

  return (
    show && (
      <Overlay
        blur={2}
        w={"100%"}
        h={"100%"}
        style={{ position: "absolute" }}
        color="#fff"
      >
        <Group h={"100%"} justify="center" align="center">
          <Badge size="xl" color="green">
            Success
          </Badge>
        </Group>
      </Overlay>
    )
  );
}

export default TimedOverlay;
