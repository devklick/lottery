import { useQuery } from "@tanstack/react-query";
import { useParams } from "react-router-dom";
import dateFormat from "dateformat";
import gameService from "../gameService";
import {
  Center,
  Container,
  Grid,
  Group,
  Paper,
  Skeleton,
  Text,
  Title,
} from "@mantine/core";
import { GetGameResponse } from "./game.schema";
import React from "react";
import CreateEntry from "./CreateEntry";
import YourEntries from "./YourEntries";
import Trophy from "../../components/Trophy/Trophy";
import GameStatusBadge from "../../components/GameStatusBadge";

const placeholders: GetGameResponse = {
  name: "Dummy Text",
  drawTime: new Date(),
  id: "dummy-id",
  state: "enabled",
  closeTime: new Date(),
  resultedAt: new Date(),
  selectionsRequiredForEntry: 5,
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

interface GameDetailProps {}

function GameDetail({}: GameDetailProps) {
  const { id } = useParams<Params>();

  const query = useQuery({
    queryKey: ["game", id],
    queryFn: async () => await gameService.getGame({ route: { id: id! } }),
    refetchInterval: 0,
  });

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

  const prizes = (query.data?.prizes ?? placeholders.prizes)
    .sort((a, b) => a.position - b.position)
    .map((prize, index) => (
      <React.Fragment key={`prize-${index}`}>
        <Grid.Col key={`prize-${index}-position`} span={6}>
          <Center>
            <Trophy loading={loading} position={prize.position} />
          </Center>
        </Grid.Col>
        <Grid.Col key={`prize-${index}-numberMatchCount`} span={6}>
          <Skeleton visible={loading}>
            <Text>
              {`${prize.numberMatchCount}/${
                query.data?.selectionsRequiredForEntry ??
                placeholders.selectionsRequiredForEntry
              }`}
            </Text>
          </Skeleton>
        </Grid.Col>
      </React.Fragment>
    ));

  return (
    <Container p={0}>
      <Group justify="center">
        <Skeleton visible={loading}>
          <Title>{query.data?.name ?? placeholders.name}</Title>
        </Skeleton>
      </Group>

      <Paper shadow="xl" p={24} radius={10}>
        <GameStatusBadge
          loading={query.isLoading}
          state={query.data?.gameStatus}
        />
        <Grid gap={{ base: 24, md: "xl", xl: 50 }} justify={"center"}>
          <Grid.Col span={{ xs: 12, sm: 4, md: 4, lg: 4, xl: 4 }}>
            <Skeleton visible={loading}>
              <Group>{startTime}</Group>
            </Skeleton>
          </Grid.Col>
          <Grid.Col span={{ xs: 12, sm: 4, md: 4, lg: 4, xl: 4 }}>
            <Skeleton visible={loading}>
              <Group>{closeTime}</Group>
            </Skeleton>
          </Grid.Col>
          <Grid.Col span={{ xs: 12, sm: 4, md: 4, lg: 4, xl: 4 }}>
            <Skeleton visible={loading}>
              <Group>{drawTime}</Group>
            </Skeleton>
          </Grid.Col>
          <Grid.Col span={12} maw={500} mx="auto">
            <Group>
              <Skeleton visible={loading}>
                <Title size={"h2"}>Prizes</Title>
              </Skeleton>
              <Grid w={"100%"}>{prizes}</Grid>
            </Group>
          </Grid.Col>
        </Grid>

        {
          <YourEntries
            gameId={id!}
            winningSelections={query.data?.results}
            gameSelections={query.data?.selections ?? []}
            gameStatus={query.data?.gameStatus ?? "closed"}
          />
        }

        {query.data?.gameStatus == "open" && (
          <CreateEntry
            gameId={id!}
            selectionNumbers={query.data?.selections.map(
              (s) => s.selectionNumber,
            )}
            selectionsRequired={query.data?.selectionsRequiredForEntry!}
          />
        )}
      </Paper>
    </Container>
  );
}

export default GameDetail;
