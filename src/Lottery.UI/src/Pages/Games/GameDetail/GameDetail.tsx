import {
  Container,
  Grid,
  Group,
  Paper,
  Skeleton,
  Stack,
  Text,
  Title,
} from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { useQuery } from "@tanstack/react-query";
import dateFormat from "dateformat";
import { useState } from "react";
import { useParams } from "react-router-dom";

import gameService from "../gameService";
import CreateEntry from "./CreateEntry";
import { GetGameResponse } from "./game.schema";
import GamePrizes from "./GamePrizes";
import YourEntries from "./YourEntries";
import GameStatusBadge from "../../../components/GameStatusBadge";
import ManageGameButton from "../../../components/ManageGameButton/ManageGameButton";
import { useUserStore } from "../../../stores/user.store";
import ResultGameModal from "../ResultGameModal";

const placeholders: GetGameResponse = {
  name: "Dummy Text",
  drawTime: new Date(),
  id: "dummy-id",
  state: "enabled",
  closeTime: new Date(),
  resultedAt: new Date(),
  selectionsRequiredForEntry: 5,
  maxEntriesPerPlayer: 10,
  startTime: new Date(),
  gameStatus: "open",
  results: [
    { id: "result-1", selectionNumber: 1 },
    { id: "result-2", selectionNumber: 2 },
    { id: "result-3", selectionNumber: 3 },
  ],
  prizes: [
    { id: "prize-1", position: 1, numberMatchCount: 5 },
    { id: "prize-2", position: 3, numberMatchCount: 4 },
    { id: "prize-3", position: 2, numberMatchCount: 3 },
  ],
  selections: [
    { id: "selection-1", selectionNumber: 1 },
    { id: "selection-2", selectionNumber: 2 },
    { id: "selection-3", selectionNumber: 3 },
    { id: "selection-4", selectionNumber: 4 },
    { id: "selection-5", selectionNumber: 5 },
  ],
};

interface Params extends Record<string, string | undefined> {
  id: string;
}

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface GameDetailProps {}

// eslint-disable-next-line no-empty-pattern
function GameDetail({}: GameDetailProps) {
  const { id } = useParams<Params>() as Params;
  const [totalEntries, setTotalEntries] = useState(0);

  const [resultGameOpened, { close: closeResultGame, open: openResultGame }] =
    useDisclosure(false);

  const query = useQuery({
    queryKey: ["game", id],
    queryFn: async () => await gameService.getGame({ route: { id } }),
    refetchInterval: 0,
  });

  const isUserType = useUserStore((s) => s.isUserType);

  const startTime = (
    <>
      <Text c={"dimmed"}>Enter from:</Text>
      {dateFormat(
        query.data?.startTime ?? placeholders.startTime,
        "dd/mm/yyyy HH:MM",
      )}
    </>
  );

  const closeTime = (
    <>
      <Text c={"dimmed"}>Closes at:</Text>
      <Text>
        {dateFormat(
          query.data?.closeTime ?? placeholders.closeTime,
          "dd/mm/yyyy HH:MM",
        )}
      </Text>
    </>
  );

  const drawTime = (
    <>
      <Text c={"dimmed"}>Draw at:</Text>
      <Text>
        {dateFormat(
          query.data?.drawTime ?? placeholders.drawTime,
          "dd/mm/yyyy HH:MM",
        )}
      </Text>
    </>
  );

  const loading = query.isLoading;
  const maxEntriesReached =
    totalEntries >= (query.data?.maxEntriesPerPlayer ?? 0);

  return (
    <Container p={0}>
      <ResultGameModal
        numbersRequired={query.data?.selectionsRequiredForEntry ?? 0}
        selectionNumbers={
          query.data?.selections.map((s) => s.selectionNumber) ?? []
        }
        gameId={id}
        onSubmit={closeResultGame}
        isOpen={resultGameOpened}
      />
      <Stack gap={24}>
        <Group justify="start">
          <Skeleton visible={loading} flex={"1 1 0"}>
            <Title style={{ textAlign: "start" }}>
              {query.data?.name ?? placeholders.name}
            </Title>
          </Skeleton>
          <Stack gap={"xs"}>
            <GameStatusBadge
              loading={query.isLoading}
              state={query.data?.gameStatus}
            />
            {isUserType("Admin") && (
              <ManageGameButton
                gameId={id}
                gameStatus={query.data?.gameStatus ?? "closed"}
                openResultGame={openResultGame}
                width="fit-content"
              />
            )}
          </Stack>
        </Group>

        <Paper shadow="xl" p={24} radius={10}>
          <Grid gap={"sm"} justify={"center"}>
            <Grid.Col span={{ xs: 4, base: 12 }}>
              <Skeleton visible={loading}>
                <Group>{startTime}</Group>
              </Skeleton>
            </Grid.Col>
            <Grid.Col span={{ xs: 4, base: 12 }}>
              <Skeleton visible={loading}>
                <Group>{closeTime}</Group>
              </Skeleton>
            </Grid.Col>
            <Grid.Col span={{ xs: 4, base: 12 }}>
              <Skeleton visible={loading}>
                <Group>{drawTime}</Group>
              </Skeleton>
            </Grid.Col>
          </Grid>
        </Paper>

        <GamePrizes
          loading={loading}
          prizes={query.data?.prizes ?? placeholders.prizes}
          selectionDrawCount={
            query.data?.selectionsRequiredForEntry ??
            placeholders.selectionsRequiredForEntry
          }
        />

        <CreateEntry
          gameId={id}
          selectionNumbers={
            query.data?.selections.map((s) => s.selectionNumber) ?? []
          }
          selectionsRequired={query.data?.selectionsRequiredForEntry ?? 0}
          gameStatus={query.data?.gameStatus ?? placeholders.gameStatus}
          maxEntriesReached={maxEntriesReached}
        />

        <YourEntries
          gameId={id}
          winningSelections={query.data?.results}
          gameSelections={query.data?.selections ?? []}
          gameStatus={query.data?.gameStatus ?? "closed"}
          onEntryCountReceived={setTotalEntries}
        />
      </Stack>
    </Container>
  );
}

export default GameDetail;
