import {
  Badge,
  Group,
  MantineColor,
  Skeleton,
  StyleProp,
  Tooltip,
} from "@mantine/core";
import clsx from "clsx";

import { GameStatus } from "../../common/schemas";

interface GameStatusBadgeProps {
  loading: boolean;
  state: GameStatus | undefined;
  groupProps?: {
    w?: StyleProp<React.CSSProperties["width"]>;
    justify?: React.CSSProperties["justifyContent"];
  };
  position?: "absolute";
}

function getStatusColor(status: GameStatus): MantineColor {
  switch (status) {
    case "closed":
      return "orange";
    case "future":
      return "blue";
    case "open":
      return "green";
    case "resulted":
      return "grey";
  }
}

const statusDescription: Record<GameStatus, string> = {
  closed:
    "The game can no longer be entered and is waiting for the results to be drawn",
  future:
    "The game is scheduled to start in the future and cannot be entered until that time",
  open: "The game is currently accepting entries",
  resulted: "The game has finished and the results have been drawn",
};

function GameStatusBadge({
  loading,
  groupProps = { justify: "end" },
  state = "resulted",
  position,
}: GameStatusBadgeProps) {
  return (
    <Group
      justify={groupProps?.justify}
      w={groupProps?.w}
      flex={"0 1 0"}
      className={clsx("game-status-badge", {
        [`game-status-badge--${state}`]: !!state,
      })}
    >
      <Skeleton w={100} visible={loading}>
        <Tooltip label={statusDescription[state]}>
          <Badge pos={position} top={0} fullWidth color={getStatusColor(state)}>
            {state}
          </Badge>
        </Tooltip>
      </Skeleton>
    </Group>
  );
}

export default GameStatusBadge;
