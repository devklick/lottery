import {
  Badge,
  Group,
  HoverCard,
  MantineColor,
  Skeleton,
  StyleProp,
  Text,
} from "@mantine/core";
import { GameStatus } from "../../common/schemas";
import { useEffect } from "react";

interface GameStatusBadgeProps {
  loading: boolean;
  state: GameStatus | undefined;
  groupProps?: {
    w?: StyleProp<React.CSSProperties["width"]>;
    justify?: React.CSSProperties["justifyContent"];
  };
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
}: GameStatusBadgeProps) {
  useEffect(() => {
    console.log("GameStatusBadge, state", state);
  }, [state]);
  return (
    <Group justify={groupProps?.justify} w={groupProps?.w}>
      <Skeleton w={100} visible={loading}>
        <HoverCard width={280} shadow="md">
          <HoverCard.Target>
            <Badge color={getStatusColor(state)}>{state}</Badge>
          </HoverCard.Target>
          <HoverCard.Dropdown>
            <Text size="sm">{statusDescription[state]}</Text>
          </HoverCard.Dropdown>
        </HoverCard>
      </Skeleton>
    </Group>
  );
}

export default GameStatusBadge;
