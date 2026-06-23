import { Button, Card, Group, Skeleton, Stack, Text } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { useNavigate } from "react-router-dom";

import ResultGameModal from "./ResultGameModal";
import { GameStatus } from "../../common/schemas";
import GameStatusBadge from "../../components/GameStatusBadge";
import ManageGameButton from "../../components/ManageGameButton";
import { useUserStore } from "../../stores/user.store";

interface GameCardProps {
  id: string;
  name: string;
  startTime: Date;
  closeTime: Date;
  drawTime: Date;
  loading: boolean;
  gameStatus: GameStatus;
  numbersRequired: number;
  selectionNumbers: Array<number>;
}

function formatDate(date: Date) {
  const formattedTime = date.toLocaleString("en-GB", { timeStyle: "short" });
  const formattedDate = date.toLocaleString("en-GB", {
    month: "short",
    day: "2-digit",
  });
  return `${formattedTime} - ${formattedDate}`;
}

function GameCard({
  id,
  name,
  startTime,
  closeTime,
  drawTime,
  gameStatus,
  loading,
  numbersRequired,
  selectionNumbers,
}: GameCardProps) {
  const navigate = useNavigate();
  const { isUserType } = useUserStore();
  const [resultGameOpened, { close: closeResultGame, open: openResultGame }] =
    useDisclosure(false);

  return (
    <>
      <ResultGameModal
        numbersRequired={numbersRequired}
        selectionNumbers={selectionNumbers}
        gameId={id}
        onSubmit={closeResultGame}
        isOpen={resultGameOpened}
      />
      <Card
        withBorder
        shadow="xl"
        radius="lg"
        h={"100%"}
        style={{ position: "relative" }}
      >
        <Card.Section withBorder inheritPadding py={"xs"}>
          <Group>
            <Skeleton visible={loading}>
              <Text fw={500}>{name}</Text>
            </Skeleton>
          </Group>
          <GameStatusBadge
            loading={loading}
            state={gameStatus}
            groupProps={{ justify: "center", w: "100%" }}
            position="absolute"
          />
        </Card.Section>

        <Card.Section h={"100%"} withBorder inheritPadding py={"xs"}>
          <Stack py={"xs"} gap={"xs"} align="start">
            <Skeleton visible={loading}>
              <Group>
                <Text c="dimmed">Starts on:</Text>
                <Text>{formatDate(startTime)}</Text>
              </Group>
            </Skeleton>
            <Skeleton visible={loading}>
              <Group>
                <Text c="dimmed">Closes on:</Text>
                <Text>{formatDate(closeTime)}</Text>
              </Group>
            </Skeleton>
            <Skeleton visible={loading}>
              <Group>
                <Text c="dimmed">Draws on:</Text>
                <Text>{formatDate(drawTime)}</Text>
              </Group>
            </Skeleton>
          </Stack>
        </Card.Section>

        <Card.Section withBorder inheritPadding py={"xs"}>
          <Stack h={"100%"} justify="flex-end">
            <Skeleton visible={loading}>
              <Group>
                <Button
                  fullWidth
                  onClick={() => navigate(`/games/${id}`)}
                  variant={gameStatus === "open" ? "gradient" : "filled"}
                  gradient={{ from: "blue", to: "grape", deg: 20 }}
                  color={gameStatus === "open" ? "green" : "blue"}
                >
                  {gameStatus == "open" ? "Play" : "View"}
                </Button>
              </Group>
            </Skeleton>
            {isUserType("Admin") && (
              <Skeleton visible={loading}>
                <ManageGameButton
                  gameId={id}
                  gameStatus={gameStatus}
                  openResultGame={openResultGame}
                  width="full"
                />
              </Skeleton>
            )}
          </Stack>
        </Card.Section>
      </Card>
    </>
  );
}

export default GameCard;
