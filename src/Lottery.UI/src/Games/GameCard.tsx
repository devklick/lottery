import {
  Button,
  Card,
  Group,
  Menu,
  Modal,
  Skeleton,
  Stack,
  Text,
  useMantineTheme,
} from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { IconBriefcase, IconEdit, IconRotate2 } from "@tabler/icons-react";
import { useNavigate } from "react-router-dom";

import { useUserStore } from "../stores/user.store";
import GameStatusBadge from "../components/GameStatusBadge";
import { GameStatus } from "../common/schemas";
import ResultGame from "./ResultGame/ResultGame";

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
  const theme = useMantineTheme();
  const { isUserType } = useUserStore();
  const [resultGameOpened, { close: closeResultGame, open: openResultGame }] =
    useDisclosure(false);

  return (
    <>
      {resultGameOpened && (
        <Modal opened={resultGameOpened} onClose={closeResultGame}>
          <ResultGame
            numbersRequired={numbersRequired}
            selectionNumbers={selectionNumbers}
            gameId={id}
            onDone={closeResultGame}
          />
        </Modal>
      )}
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
                  variant={gameStatus === "open" ? "filled" : "filled"}
                  // bd={gameStatus === "open" ? "2px solid white" : undefined}
                  color={gameStatus === "open" ? "green" : "blue"}
                >
                  {gameStatus == "open" ? "Play" : "View"}
                </Button>
              </Group>
            </Skeleton>
            {isUserType("Admin") && (
              <Skeleton visible={loading}>
                <Group justify="space-between">
                  <Menu withinPortal shadow="sm">
                    <Menu.Target>
                      <Button
                        fullWidth
                        color={"gray"}
                        leftSection={<IconBriefcase size={18} />}
                      >
                        Manage
                      </Button>
                    </Menu.Target>
                    <Menu.Dropdown>
                      <Menu.Item
                        disabled={gameStatus === "resulted"}
                        leftSection={<IconEdit size={18} />}
                        onClick={() => navigate(`/games/${id}/edit`)}
                      >
                        Edit
                      </Menu.Item>
                      <Menu.Item
                        disabled={gameStatus !== "closed"}
                        leftSection={<IconRotate2 size={18} />}
                        onClick={openResultGame}
                      >
                        Result
                      </Menu.Item>
                    </Menu.Dropdown>
                  </Menu>
                </Group>
              </Skeleton>
            )}
          </Stack>
        </Card.Section>
      </Card>
    </>
  );
}

export default GameCard;
