import {
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
import { IconChevronDown, IconChevronUp } from "@tabler/icons-react";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import gameService from "../gameService";
import Trophy from "../../components/Trophy/Trophy";

interface YourEntriesProps {
  winningSelections?: Array<{ id: string; selectionNumber: number }>;
  gamePrizes: Array<{ id: string; position: number; numberMatchCount: number }>;
  gameId: string;
}

function YourEntries({
  gameId,
  winningSelections,
  gamePrizes,
}: YourEntriesProps) {
  const user = useUserStore();
  const [opened, { toggle }] = useDisclosure(false);
  const [page, setPage] = useState(1);
  const [limit, setLimit] = useState(5);

  const query = useQuery({
    queryKey: ["entries", page, limit, gameId],
    queryFn: () => gameService.getEntries({ query: { limit, page, gameId } }),
    enabled: user.authenticated,
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

  const entries = query.data?.items.map((entry) => (
    <Skeleton key={entry.id} visible={query.isLoading}>
      <Group>
        {entry.selections
          .sort((a, b) => a.selectionNumber - b.selectionNumber)
          .map((selection) => getSelection(entry.id, selection))}
        {getTrophy(entry.selections)}
      </Group>
    </Skeleton>
  ));

  const totalPages = Math.max(Math.ceil((query.data?.total ?? 0) / limit), 1);

  const paginaton = (
    <Flex gap={"lg"} align={"center"}>
      <Pagination total={totalPages} value={page} onChange={setPage} />
      <Select
        w={80}
        value={limit.toString()}
        defaultValue={limit.toString()}
        data={["5", "10", "20"]}
        onChange={(value) => setLimit(Number(value))}
        allowDeselect={false}
      />
    </Flex>
  );

  return (
    <Stack align="center" justify="center" mt={50}>
      <Group style={{ alignSelf: "start" }} onClick={toggle}>
        <Title size={"h2"}>{`Your entries`}</Title>
        {opened ? <IconChevronUp /> : <IconChevronDown />}
      </Group>
      <Collapse in={opened}>
        {!user.authenticated ? (
          <Text span>
            <Anchor href="/account/signIn">Sign in</Anchor> to view your entries
          </Text>
        ) : (
          <Center w={"100%"} mt={50}>
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
