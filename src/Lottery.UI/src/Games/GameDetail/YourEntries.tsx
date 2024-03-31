import {
  ActionIcon,
  Anchor,
  Badge,
  Center,
  Collapse,
  Flex,
  Group,
  MantineColor,
  Pagination,
  Select,
  Skeleton,
  Stack,
  Text,
  Title,
} from "@mantine/core";
import { useUserStore } from "../../stores/user.store";
import { useDisclosure } from "@mantine/hooks";
import { IconChevronDown, IconChevronUp, IconEdit } from "@tabler/icons-react";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import gameService from "../gameService";
import Trophy from "../../components/Trophy/Trophy";
import EditEntry from "./EditEntry";

interface YourEntriesProps {
  gameSelections: ReadonlyArray<{ id: string; selectionNumber: number }>;
  winningSelections?: ReadonlyArray<{ id: string; selectionNumber: number }>;
  gamePrizes: ReadonlyArray<{
    id: string;
    position: number;
    numberMatchCount: number;
  }>;
  gameId: string;
}

function YourEntries({
  gameId,
  winningSelections,
  gamePrizes,
  gameSelections,
}: YourEntriesProps) {
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

  function getTrophy(
    selections: Array<{ id: string; selectionNumber: number }>
  ) {
    if (!winningSelections?.length) return null;

    const matched =
      winningSelections?.filter((ws) => selections.some((s) => ws.id === s.id))
        ?.length ?? 0;

    for (const prize of gamePrizes) {
      if (prize.numberMatchCount === matched) {
        return <Trophy position={prize.position} loading={false} />;
      }
    }

    return <Trophy position={0} loading={false} disabled />;
  }

  function getSelectionColor(selectionId: string): MantineColor {
    if (!winningSelections?.length) return "blue";

    return winningSelections.find((ws) => ws.id === selectionId)
      ? "green"
      : "gray";
  }

  function getSelection(
    entryId: string,
    selection: { id: string; selectionNumber: number }
  ) {
    return (
      <Badge
        key={`${entryId}-${selection.id}`}
        circle
        size={"xl"}
        color={getSelectionColor(selection.id)}
      >
        {selection.selectionNumber}
      </Badge>
    );
  }

  function removeCurrentQuery() {
    queryClient.removeQueries({ queryKey: ["entries", gameId, page, limit] });
  }

  function handleLimitChanged(value: string | null) {
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
        {getTrophy(entry.selections)}
        <ActionIcon
          variant="subtle"
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

  const totalPages = Math.max(Math.ceil((query.data?.total ?? 0) / limit), 1);

  const paginaton = (
    <Flex gap={"lg"} align={"center"}>
      <Pagination
        total={totalPages}
        value={page}
        onChange={handlePageChanged}
      />
      <Select
        w={80}
        value={limit.toString()}
        defaultValue={limit.toString()}
        data={["5", "10", "20"]}
        onChange={handleLimitChanged}
        allowDeselect={false}
      />
    </Flex>
  );

  function handleEntryEdited() {
    setEditTarget(null);
  }

  return (
    <Stack align="center" justify="center" mt={50}>
      <Group style={{ alignSelf: "start" }} onClick={toggle}>
        <Title size={"h2"}>{`Your entries`}</Title>
        {opened ? <IconChevronUp /> : <IconChevronDown />}
      </Group>
      <Collapse in={opened}>
        {!user.authenticated() ? (
          <Text span>
            <Anchor href="/account/signIn">Sign in</Anchor> to view your entries
          </Text>
        ) : (
          <Center w={"100%"} mt={50}>
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
              {paginaton}
            </Stack>
          </Center>
        )}
      </Collapse>
    </Stack>
  );
}

export default YourEntries;
