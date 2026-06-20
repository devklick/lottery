import PageSection from "../components/PageSection";

interface YourEntriesProps {}

export default function YourEntries({}: YourEntriesProps) {
  return (
    <PageSection
      title="Your Entries"
      subheader="Here you can find all entries you have placed"
      collapsable
    />
  );
}
