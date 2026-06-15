import {
  ActionIcon,
  Anchor,
  Center,
  Collapse,
  Flex,
  Group,
  MantineColorsTuple,
  Pagination,
  Select,
  Skeleton,
  Stack,
  Text,
  Title,
  useMantineTheme,
} from "@mantine/core";
import { useUserStore } from "../../stores/user.store";
import { useDisclosure } from "@mantine/hooks";
import { IconChevronDown, IconChevronUp, IconEdit } from "@tabler/icons-react";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import gameService from "../gameService";
import Trophy from "../../components/Trophy/Trophy";
import EditEntry from "./EditEntry";
import { GameStatus } from "../../common/schemas";
import { EntryPrize } from "./game.schema";
import NumberBall from "../../components/NumberBall/NumberBall";
import PageSection from "../../components/PageSection/PageSection";
import PaginationBar from "../../components/PaginationBar/PaginationBar";

interface YourEntriesProps {
  gameSelections: ReadonlyArray<{ id: string; selectionNumber: number }>;
  /**
   * The selections that were drawn when the game was resulted, AKA the results.
   */
  winningSelections?: ReadonlyArray<{ id: string; selectionNumber: number }>;
  gameId: string;
  gameStatus: GameStatus;
}

function YourEntries({
  gameId,
  winningSelections,
  gameSelections,
  gameStatus,
}: YourEntriesProps) {
  const { colors } = useMantineTheme();
  const user = useUserStore();
  const [opened, { toggle }] = useDisclosure(false);
  const [page, setPage] = useState(1);
  const [limit, setLimit] = useState(5);
  const [editTarget, setEditTarget] = useState<{
    entryId: string;
    selectionNumbers: ReadonlyArray<number>;
  } | null>(null);

  const queryClient = useQueryClient();
  const query = useQuery({
    queryKey: ["entries", gameId, page, limit],
    queryFn: () => gameService.getEntries({ query: { limit, page, gameId } }),
    enabled: user.authenticated(),
  });

  const resulted = !!winningSelections?.length;

  function getTrophy(prize: EntryPrize) {
    if (!resulted) {
      return null;
    }

    return (
      <Trophy
        position={prize?.position ?? 0}
        loading={false}
        disabled={!prize}
      />
    );
  }

  function getSelectionColorRange(selectionNumber: number): MantineColorsTuple {
    if (!resulted) return colors.blue;

    return winningSelections.find(
      (ws) => ws.selectionNumber === selectionNumber,
    )
      ? colors.green
      : colors.gray;
  }

  function getSelection(
    entryId: string,
    selection: { id: string; selectionNumber: number },
  ) {
    return (
      <NumberBall
        key={`${entryId}-${selection.id}`}
        value={selection.selectionNumber}
        selected
        colorRange={getSelectionColorRange(selection.selectionNumber)}
        cursor="default"
      />
    );
  }

  function removeCurrentQuery() {
    queryClient.removeQueries({ queryKey: ["entries", gameId, page, limit] });
  }

  function handleLimitChanged(value: number | null) {
    removeCurrentQuery();
    setLimit(Number(value));
  }

  function handlePageChanged(value: number) {
    removeCurrentQuery();
    setPage(value);
  }

  const entries = query.data?.items.map((entry) => (
    <Skeleton key={entry.id} visible={query.isLoading}>
      <Group justify="center">
        {entry.selections
          .sort((a, b) => a.selectionNumber - b.selectionNumber)
          .map((selection) => getSelection(entry.id, selection))}
        {getTrophy(entry.prize)}
        <ActionIcon
          variant="subtle"
          disabled={gameStatus !== "open"}
          onClick={() =>
            setEditTarget({
              entryId: entry.id,
              selectionNumbers: entry.selections.map((s) => s.selectionNumber),
            })
          }
        >
          <IconEdit />
        </ActionIcon>
      </Group>
    </Skeleton>
  ));

  const subheader = (() => {
    if (entries?.length) return;
    if (gameStatus === "closed" || gameStatus === "resulted")
      return "You did not enter this game";
    if (gameStatus === "future")
      return "Come back later when the game opens to submit your entry!";
    return "You have not yet entered this game. Pick your numbers below!";
  })();

  const totalPages = Math.max(Math.ceil((query.data?.total ?? 0) / limit), 1);

  function handleEntryEdited() {
    setEditTarget(null);
  }

  return (
    <PageSection
      title="Your Entries"
      collapsable
      subheader={subheader}
      className="your-entries-page-section"
    >
      {!user.authenticated() ? (
        <Text span>
          <Anchor href="/account/signIn">Sign in</Anchor> to view your entries
        </Text>
      ) : (
        <Center w={"100%"}>
          {editTarget && (
            <EditEntry
              selectedNumbers={editTarget.selectionNumbers}
              selectionNumbers={gameSelections.map((s) => s.selectionNumber)}
              onClose={handleEntryEdited}
              entryId={editTarget.entryId}
            />
          )}
          <Stack align="center">
            {entries}
            <PaginationBar
              limit={limit}
              onLimitChanged={handleLimitChanged}
              onPageChanged={handlePageChanged}
              page={page}
              totalPages={totalPages}
            />
          </Stack>
        </Center>
      )}
    </PageSection>
  );
}

export default YourEntries;
